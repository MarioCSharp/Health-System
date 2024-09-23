using HealthSystem.Booking.Data;
using HealthSystem.Booking.Data.Models;
using HealthSystem.Booking.Models;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Linq;

namespace HealthSystem.Booking.Services.AppointmentService
{
    /// <summary>
    /// Service for managing appointments and related operations.
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private BookingDbContext context;
        private HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentService"/> class.
        /// </summary>
        /// <param name="context">The database context for bookings and appointments.</param>
        /// <param name="httpClient">The HTTP client used for external service requests.</param>
        public AppointmentService(BookingDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Adds a comment to an appointment.
        /// </summary>
        /// <param name="model">The model containing the comment details.</param>
        /// <param name="userId">The ID of the user (doctor) making the comment.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean value indicating success or failure.</returns>
        public async Task<bool> AddComment(AppointmentCommentAddModel model, string userId)
        {
            var doctorResponse = await httpClient.GetAsync($"http://admins/api/Doctor/GetDoctorByUserId?userId={userId}");

            if (!doctorResponse.IsSuccessStatusCode)
            {
                return false;
            }

            var doctor = await doctorResponse.Content.ReadFromJsonAsync<DoctorModel>();

            if (doctor == null)
            {
                return false;
            }

            var appointment = await context.Bookings.FindAsync(model.AppointmentId);

            if (appointment is null || appointment.DoctorId != doctor.Id)
            {
                return false;
            }

            var comment = new AppointmentComment()
            {
                Comment = model.Comment,
                AppointmentId = model.AppointmentId
            };

            await context.AppointmentComments.AddAsync(comment);
            await context.SaveChangesAsync();

            return await context.AppointmentComments.ContainsAsync(comment);
        }

        /// <summary>
        /// Deletes all appointments for a specified doctor.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor whose appointments are to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAllByDoctorId(int doctorId)
        {
            await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Bookings WHERE DoctorId = {doctorId}");
        }

        /// <summary>
        /// Gets the details of an appointment by ID.
        /// </summary>
        /// <param name="id">The ID of the appointment.</param>
        /// <returns>A task representing the asynchronous operation, containing an <see cref="AppointmentReturnModel"/> with the appointment details.</returns>
        public async Task<AppointmentReturnModel> GetAppointment(int id)
        {
            var app = await context.Bookings.FindAsync(id);

            if (app == null)
            {
                return new AppointmentReturnModel();
            }

            return new AppointmentReturnModel()
            {
                DoctorId = app.DoctorId,
                UserId = app.UserId
            };
        }

        /// <summary>
        /// Gets a list of appointments for a specified doctor.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor whose appointments are to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, containing the doctor's name and a list of appointments.</returns>
        public async Task<(string, List<BookingDisplayModel>)> GetDoctorAppointments(int doctorId)
        {
            var apps = await context.Bookings
                    .Where(x => x.DoctorId == doctorId).Include(x => x.Service).ToListAsync();

            var result = new List<BookingDisplayModel>();
            var doctorName = string.Empty;

            foreach (var app in apps)
            {
                if (string.IsNullOrEmpty(doctorName))
                {
                    doctorName = app.DoctorName;
                }

                result.Add(new BookingDisplayModel()
                {
                    Id = app.Id,
                    ServiceName = app.Service.Name,
                    Date = app.Date.ToString("dd/MM/yyyy HH:mm"),
                    Name = app.PatientName,
                });
            }

            return (doctorName ?? "", result);
        }

        /// <summary>
        /// Gets a list of upcoming appointments for a doctor by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the doctor (user).</param>
        /// <returns>A task representing the asynchronous operation, containing a list of upcoming appointments.</returns>
        public async Task<List<AppointmentPatientModel>> GetNextAppointmentsByDoctorUserId(string userId)
        {
            var doctorResponse = await httpClient.GetAsync($"http://admins/api/Doctor/GetDoctorByUserId?userId={userId}");

            if (!doctorResponse.IsSuccessStatusCode)
            {
                return new List<AppointmentPatientModel>();
            }

            var doctor = await doctorResponse.Content.ReadFromJsonAsync<DoctorModel>();

            if (doctor == null)
            {
                return new List<AppointmentPatientModel>();
            }

            return await context.Bookings
                .Where(x => x.DoctorId == doctor.Id && x.Date > DateTime.Now)
                .OrderBy(x => x.Date)
                .Select(x => new AppointmentPatientModel
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                    PatientName = x.PatientName,
                    ServiceName = x.Service.Name
                }).ToListAsync();
        }

        /// <summary>
        /// Gets a list of past appointments for a doctor by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the doctor (user).</param>
        /// <returns>A task representing the asynchronous operation, containing a list of past appointments.</returns>
        public async Task<List<AppointmentPatientModel>> GetPastAppointmentsByDoctorUserId(string userId)
        {
            var doctorResponse = await httpClient.GetAsync($"http://admins/api/Doctor/GetDoctorByUserId?userId={userId}");

            if (!doctorResponse.IsSuccessStatusCode)
            {
                return new List<AppointmentPatientModel>();
            }

            var doctor = await doctorResponse.Content.ReadFromJsonAsync<DoctorModel>();

            if (doctor == null)
            {
                return new List<AppointmentPatientModel>();
            }

            return await context.Bookings
                .Where(x => x.DoctorId == doctor.Id && x.Date < DateTime.Now)
                .OrderBy(x => x.Date)
                .Select(x => new AppointmentPatientModel
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                    PatientName = x.PatientName,
                    ServiceName = x.Service.Name
                }).ToListAsync();
        }

        /// <summary>
        /// Gets a list of appointments for a user by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task representing the asynchronous operation, containing the user's name and a list of their appointments.</returns>
        public async Task<(string, List<BookingDisplayModel>)> GetUserAppointments(string userId)
        {
            var apps = context.Bookings
                .Where(x => x.UserId == userId)
                .Include(x => x.Service);

            var result = new List<BookingDisplayModel>();

            var userName = "";

            foreach (var appointment in apps)
            {
                if (string.IsNullOrEmpty(userName))
                {
                    userName = appointment.PatientName;
                }

                result.Add(new BookingDisplayModel
                {
                    Id = appointment.Id,
                    Date = appointment.Date.ToString("dd/MM/yyyy HH:mm"),
                    Name = appointment.DoctorName,
                    ServiceName = appointment.Service.Name
                });
            }

            return (userName, result);
        }

        /// <summary>
        /// Gets a list of prescriptions for a user by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of prescription display models.</returns>
        public async Task<List<PrescriptionDisplayModel>> GetUserPrescriptions(string userId)
        {
            var appointments = await context.Bookings
                            .Where(x => x.UserId == userId)
                            .ToListAsync();

            var result = new List<PrescriptionDisplayModel>();
            var allPrescriptions = await context.AppointmentPrescriptions.ToListAsync();

            foreach (var appointment in appointments)
            {
                var pres = allPrescriptions.FirstOrDefault(x => x.AppointmentId == appointment.Id);

                if (pres is not null)
                {
                    result.Add(new PrescriptionDisplayModel()
                    {
                        Date = appointment.Date.ToString("dd/MM/yyyy HH:mm"),
                        DoctorName = appointment.DoctorName,
                        File = pres.File
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the next three appointments for a user by their user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of upcoming appointment models.</returns>
        public async Task<List<AppointmentModel>> GetUsersNextAppointments(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new List<AppointmentModel>();
            }

            return await context.Bookings
                .Where(x => x.UserId == userId && x.Date > DateTime.Now)
                .OrderBy(x => x.Date)
                .Take(3)
                .Select(x => new AppointmentModel
                {
                    Id = x.Id,
                    ServiceName = x.Service.Name,
                    DoctorName = x.DoctorName,
                    Date = x.Date.ToString("dd/MM/yyyy HH:mm")
                })
                .ToListAsync();
        }

        /// <summary>
        /// Checks if an appointment has an associated prescription.
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean indicating if a prescription exists and the associated file.</returns>
        public async Task<(bool, IFormFile)> HasPrescriptionAsync(int appointmentId)
        {
            var appointment = await context.Bookings.FindAsync(appointmentId);

            if (appointment == null)
            {
                return (false, null);
            }

            var appointmentPrescription = await context.AppointmentPrescriptions
                .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);

            if (appointmentPrescription == null)
            {
                return (false, null);
            }

            var stream = new MemoryStream(appointmentPrescription.File);

            var formFile = new FormFile(stream, 0, appointmentPrescription.File.Length, "file", "Амбулаторен лист")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            return (true, formFile);
        }
        /// <summary>
        /// Generates a prescription file.
        /// </summary>
        /// <param name="model">The model for the creation.</param>
        /// <param name="userId">The user which the prescription belongs to.</param>
        /// <returns>Tupple which is (bool, IFormFile). The bool is saying if everything went ok and the file(if it was created).</returns>

        public async Task<(bool, IFormFile)> IssuePrescriptionAsync(PrescriptionModel model, string userId)
        {
            var appointment = await context.Bookings.FindAsync(model.AppointmentId);

            if (appointment == null)
            {
                return (false, null);
            }

            var doctorResponse = await httpClient.GetAsync($"http://admins/api/Doctor/GetDoctorByUserId?userId={userId}");

            if (!doctorResponse.IsSuccessStatusCode)
            {
                return (false, null);
            }

            var doctor = await doctorResponse.Content.ReadFromJsonAsync<DoctorModel>();

            if (doctor == null || doctor.Id != appointment.DoctorId)
            {
                return (false, null);
            }

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Амбулаторен лист";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titleFont = new XFont("Verdana", 14, XFontStyle.Bold);
            XFont normalFont = new XFont("Verdana", 8, XFontStyle.Regular);
            XPen linePen = new XPen(XColors.Black, 0.5);
            int marginLeft = 40;
            int yPoint = 20;

            gfx.DrawString("АМБУЛАТОРЕН ЛИСТ", titleFont, XBrushes.Black, new XRect(0, yPoint, page.Width, 20), XStringFormats.Center);
            yPoint += 30;

            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 20);
            gfx.DrawString("Здравно заведение:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);

            yPoint += 25;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 20);
            gfx.DrawString("Отделение:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);

            yPoint += 25;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 60);
            gfx.DrawString($"Име на пациента: {model.FullName}", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString($"Дата на раждане: {model.DateOfBirth}", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 20, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString($"ЕГН: {model.EGN}", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 35, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString($"Адрес: {model.Address}", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 50, page.Width, 15), XStringFormats.TopLeft);

            yPoint += 65;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 40);
            gfx.DrawString("Оплаквания:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString(model.Complaints, normalFont, XBrushes.Black, new XRect(marginLeft + 15, yPoint + 20, page.Width - 100, 15), XStringFormats.TopLeft);

            yPoint += 45;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 40);
            gfx.DrawString("Диагноза:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString(model.Diagnosis, normalFont, XBrushes.Black, new XRect(marginLeft + 15, yPoint + 20, page.Width - 100, 15), XStringFormats.TopLeft);

            yPoint += 45;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 40);
            gfx.DrawString("Състояние:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString(model.Conditions, normalFont, XBrushes.Black, new XRect(marginLeft + 15, yPoint + 20, page.Width - 100, 15), XStringFormats.TopLeft);

            yPoint += 45;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 40);
            gfx.DrawString("Текущ статус:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString(model.Status, normalFont, XBrushes.Black, new XRect(marginLeft + 15, yPoint + 20, page.Width - 100, 15), XStringFormats.TopLeft);

            yPoint += 45;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 40);
            gfx.DrawString("Терапия:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString(model.Therapy, normalFont, XBrushes.Black, new XRect(marginLeft + 15, yPoint + 20, page.Width - 100, 15), XStringFormats.TopLeft);

            yPoint += 45;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 40);
            gfx.DrawString("Препоръчани изследвания:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString(model.Tests, normalFont, XBrushes.Black, new XRect(marginLeft + 15, yPoint + 20, page.Width - 100, 15), XStringFormats.TopLeft);

            yPoint += 45;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 30);
            gfx.DrawString($"Име на лекаря: {model.DoctorName}", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawString($"УИН: {model.UIN}", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 20, page.Width, 15), XStringFormats.TopLeft);

            yPoint += 35;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 30);
            gfx.DrawString("Подпис на лекаря:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawLine(linePen, marginLeft + 100, yPoint + 20, page.Width - marginLeft - 20, yPoint + 20);

            yPoint += 35;
            gfx.DrawRectangle(linePen, marginLeft, yPoint, page.Width - 80, 30);
            gfx.DrawString("Подпис на пациента:", normalFont, XBrushes.Black, new XRect(marginLeft + 5, yPoint + 5, page.Width, 15), XStringFormats.TopLeft);
            gfx.DrawLine(linePen, marginLeft + 100, yPoint + 20, page.Width - marginLeft - 20, yPoint + 20);

            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            stream.Position = 0;

            await context.AppointmentPrescriptions.AddAsync(new AppointmentPrescription
            {
                AppointmentId = model.AppointmentId,
                File = stream.ToArray()
            });

            await context.SaveChangesAsync();

            var file = new FormFile(stream, 0, stream.Length, "file", $"Амбулаторен_лист_{model.FullName}.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            return (true, file);
        }

        /// <summary>
        /// Removes an appointment by its ID, ensuring the user is the doctor assigned to the appointment.
        /// </summary>
        /// <param name="id">The ID of the appointment to remove.</param>
        /// <param name="userId">The ID of the doctor (user) attempting to remove the appointment.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean indicating success or failure.</returns>
        public async Task<bool> Remove(int id, string userId)
        {
            var doctorResponse = await httpClient.GetAsync($"http://admins/api/Doctor/GetDoctorByUserId?userId={userId}");

            if (!doctorResponse.IsSuccessStatusCode)
            {
                return false;
            }

            var doctor = await doctorResponse.Content.ReadFromJsonAsync<DoctorModel>();

            if (doctor == null)
            {
                return false;
            }

            var app = await context.Bookings.FindAsync(id);

            if (app is null || app.DoctorId != doctor.Id)
            {
                return false;
            }

            context.Bookings.Remove(app);
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Removes an appointment by its ID.
        /// </summary>
        /// <param name="appointmetId">The ID of the appointment to remove.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean indicating success or failure.</returns>
        public async Task<bool> RemoveAppointment(int appointmetId)
        {
            var app = await context.Bookings.FindAsync(appointmetId);

            if (app is null)
            {
                return false;
            }

            context.Bookings.Remove(app);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
