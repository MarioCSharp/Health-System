using HealthSystem.Booking.Data;
using HealthSystem.Booking.Data.Models;
using HealthSystem.Booking.Models;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace HealthSystem.Booking.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private BookingDbContext context;
        private HttpClient httpClient;

        public AppointmentService(BookingDbContext context,
                                  HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        public async Task<bool> AddComment(AppointmentCommentAddModel model, string userId)
        {
            var doctorResponse = await httpClient.GetAsync($"http://localhost:5025/api/Doctor/GetDoctorByUserId?userId={userId}");

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

        public async Task DeleteAllByDoctorId(int doctorId)
        {
            await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Bookings WHERE DoctorId = {doctorId}");
        }

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

        public async Task<(string, List<BookingDisplayModel>)> GetDoctorAppointments(int doctorId)
        {
            var doctorResponse = await httpClient.GetAsync($"http://localhost:5025/api/Doctor/GetDoctor?id={doctorId}");

            if (!doctorResponse.IsSuccessStatusCode)
            {
                return ("", new List<BookingDisplayModel>());
            }

            var doctor = await doctorResponse.Content.ReadFromJsonAsync<DoctorModel>();

            if (doctor == null)
            {
                return ("", new List<BookingDisplayModel>());
            }

            var apps = await context.Bookings
                .Where(x => x.DoctorId == doctorId)
                .Select(x => new BookingDisplayModel
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                    Name = x.PatientName,
                    ServiceName = x.Service.Name
                }).ToListAsync();

            return (doctor.FullName ?? "", apps);
        }

        public async Task<List<AppointmentPatientModel>> GetNextAppointmentsByDoctorUserId(string userId)
        {
            var doctorResponse = await httpClient.GetAsync($"http://localhost:5025/api/Doctor/GetDoctorByUserId?userId={userId}");

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

        public async Task<List<AppointmentPatientModel>> GetPastAppointmentsByDoctorUserId(string userId)
        {
            var doctorResponse = await httpClient.GetAsync($"http://localhost:5025/api/Doctor/GetDoctorByUserId?userId={userId}");

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

        public async Task<(bool, IFormFile)> IssuePrescriptionAsync(PrescriptionModel model, string userId)
        {
            var appointment = await context.Bookings.FindAsync(model.AppointmentId);

            if (appointment == null)
            {
                return (false, null);
            }

            var doctorResponse = await httpClient.GetAsync($"http://localhost:5025/api/Doctor/GetDoctorByUserId?userId={userId}");

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

        public async Task<bool> Remove(int id, string userId)
        {
            var doctorResponse = await httpClient.GetAsync($"http://localhost:5025/api/Doctor/GetDoctorByUserId?userId={userId}");

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
