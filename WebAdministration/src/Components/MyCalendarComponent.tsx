import { useEffect, useState } from "react";
import { Container, Row, Col, Table, Button } from "react-bootstrap";
import moment from "moment";

interface Appointment {
  id: number;
  date: string; // date is a string from the API
  serviceName: string;
  name: string;
}

function MyCalendarComponent() {
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [error, setError] = useState<boolean>(false);
  const [doctorId, setDoctorId] = useState<number | null>(null);
  const [currentDate, setCurrentDate] = useState<moment.Moment>(moment());
  const token = localStorage.getItem("token");

  const getDoctorId = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Doctor/GetDoctorId`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        const data = await response.json();
        setDoctorId(data.id);
      } else {
        throw new Error("There was an error getting the doctorId");
      }
    } catch (error) {
      console.log(error);
      setError(true);
      setDoctorId(null);
    }
  };

  const getAppointments = async () => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Appointment/GetAppointments?id=${doctorId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        const data = await response.json();
        // Sort appointments by time after parsing
        const sortedAppointments = data.appointments.sort(
          (a: Appointment, b: Appointment) =>
            moment(a.date, "DD/MM/YYYY HH:mm").unix() -
            moment(b.date, "DD/MM/YYYY HH:mm").unix()
        );
        setAppointments(sortedAppointments);
        console.log("Fetched Appointments:", sortedAppointments);
      } else {
        throw new Error("There was an error loading the appointments");
      }
    } catch (error) {
      console.log(error);
      setError(true);
      setAppointments([]);
    }
  };

  useEffect(() => {
    getDoctorId();
  }, []);

  useEffect(() => {
    if (doctorId) {
      getAppointments();
    }
  }, [doctorId]);

  const renderCalendar = () => {
    const startDay = currentDate.clone().startOf("month").startOf("week");
    const endDay = currentDate.clone().endOf("month").endOf("week");

    const day = startDay.clone().subtract(1, "day");
    const calendar = [];

    while (day.isBefore(endDay, "day")) {
      calendar.push(
        Array(7)
          .fill(0)
          .map(() => day.add(1, "day").clone())
      );
    }

    return calendar;
  };

  const isSameDay = (day: moment.Moment, appointmentDateString: string) => {
    const appointmentDate = moment(appointmentDateString, "DD/MM/YYYY HH:mm"); // Parse the date string using the correct format
    return day.isSame(appointmentDate, "day");
  };

  const renderAppointments = (day: moment.Moment) => {
    const dayAppointments = appointments.filter((appointment) =>
      isSameDay(day, appointment.date)
    );

    return dayAppointments.map((appointment) => (
      <div
        key={appointment.id}
        style={{
          backgroundColor: "#f8f9fa", // Light grey background for the appointment box
          border: "1px solid #ddd", // Light border for the appointment box
          borderRadius: "4px", // Rounded corners
          padding: "5px", // Padding inside the box
          marginBottom: "5px", // Space between boxes
        }}
      >
        <small>
          <strong>{appointment.serviceName}</strong>
        </small>
        <br />
        <small>{appointment.name}</small>
        <br />
        <small>
          {moment(appointment.date, "DD/MM/YYYY HH:mm").format("HH:mm")}
        </small>{" "}
        {/* Display the time */}
      </div>
    ));
  };

  const handlePrevMonth = () => {
    setCurrentDate(currentDate.clone().subtract(1, "month"));
  };

  const handleNextMonth = () => {
    setCurrentDate(currentDate.clone().add(1, "month"));
  };

  const handlePrevYear = () => {
    setCurrentDate(currentDate.clone().subtract(1, "year"));
  };

  const handleNextYear = () => {
    setCurrentDate(currentDate.clone().add(1, "year"));
  };

  return (
    <Container>
      <Row className="mb-4">
        <Col>
          <h2 className="text-center">{currentDate.format("MMMM YYYY")}</h2>
        </Col>
      </Row>
      <Row className="mb-4">
        <Col className="text-left">
          <Button variant="primary" onClick={handlePrevYear}>
            Година назад
          </Button>
          <Button variant="primary" className="ml-2" onClick={handlePrevMonth}>
            Месец назад
          </Button>
        </Col>
        <Col className="text-right">
          <Button variant="primary" className="mr-2" onClick={handleNextMonth}>
            Месец напред
          </Button>
          <Button variant="primary" onClick={handleNextYear}>
            Година напред
          </Button>
        </Col>
      </Row>
      <Table bordered>
        <thead>
          <tr>
            <th>Неделя</th>
            <th>Понеделник</th>
            <th>Вторник</th>
            <th>Сряда</th>
            <th>Четвъртък</th>
            <th>Петък</th>
            <th>Събота</th>
          </tr>
        </thead>
        <tbody>
          {renderCalendar().map((week, index) => (
            <tr key={index}>
              {week.map((day) => (
                <td key={day.format("DD-MM-YYYY")} className="p-2">
                  <div className="text-right">
                    <strong>{day.format("D")}</strong>
                  </div>
                  <div>{renderAppointments(day)}</div>
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </Table>
      {error && (
        <div className="alert alert-danger" role="alert">
          There was an error loading the appointments.
        </div>
      )}
    </Container>
  );
}

export default MyCalendarComponent;
