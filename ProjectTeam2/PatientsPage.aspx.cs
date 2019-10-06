using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectTeam2
{
    public partial class PatientsPage : System.Web.UI.Page
    {
        //TODO create this database connection string
        private string connectionString;
        //Patient class
        public class Patient
        {
            public string fName;
            public string lName;
            public int? phoneNumber;
            public string nextAppointment;
            public int? patientId;
        }
        //Appointment class 
        public class Appointment
        {
            public string appointmentDate;
            public string appointmentDoctor;
            public string appointmentCurrentMeds;
            public string appointmentNewMedications;
            public string appointmentNotes;
            public int? appointmentId;
        }
        //Medication class
        public class Medication
        {
            public string medicationPerscriptionDate;
            public string medicationPerscriptionLength;
            public string medicationDosage;
            public bool? medicationActive;
            public string medicationNotes;
            public int? medicationId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Web method called when the main table is loaded
        /// calls getPatients(); to return a list of all patients in database
        /// </summary>
        /// <returns>List of all patients</returns>
        [WebMethod]
        public List<Patient> LoadMainTable()
        {
            return getPatients();
        }       

        /// <summary>
        /// Web method called to delete patient form database
        /// No need to return anything, we can use the success function
        /// of the ajax request to verify this functioned properly
        /// <param name="patientId">Id to be deleted</param>
        /// </summary>
        [WebMethod]
        public void DeletePatient(int patientId)
        {
            deletePatient(patientId); 
        }      
        /// <summary>
        /// Web method called to save patient information
        /// </summary>
        /// <param name="patient">the patient object to be saved</param>
        [WebMethod]
        public void SavePatient(Patient patient)
        {
            savePatient(patient);
        }

        /// <summary>
        /// Web method called when appointments table is loaded 
        /// </summary>
        /// <param name="patientId">the patient identifier to use in database look up</param>
        /// <returns>list of appointments for given patient</returns>
        [WebMethod]
        public List<Appointment> LoadAppointmentsTable( int patientId ){
            return getAppointments(patientId);
            }
        
        /// <summary>
        /// Web method called to delete appointment form database
        /// No need to return anything, we can use the success function
        /// of the ajax request to verify this functioned properly
        /// <param name="appointmentId">Id to be deleted</param>
        /// </summary>
        [WebMethod]
        public void DeleteAppointment(int appointmentId)
        {
            deleteAppointment(appointmentId);
        }        

        /// <summary>
        /// Web method called to save appointment information
        /// </summary>
        /// <param name="appointment">the appointment object to be saved</param>
        [WebMethod]
        public void SaveAppointment(Appointment appointment)
        {
            saveAppointment(appointment);
        }       

        /// <summary>
        /// Web method called when medications table is loaded
        /// </summary>
        /// <param name="patientId">the patient identifier to use in database look up</param>
        /// <returns>List of medications</returns>
        [WebMethod]
        public List<Medication> LoadMedicationsTable (int patientId)
        {
            return getMedications(patientId);
        }
        
        /// <summary>
        /// Web method called to delete medication form database
        /// No need to return anything, we can use the success function
        /// of the ajax request to verify this functioned properly
        /// <param name="medicationId">Id to be deleted</param>
        /// </summary>
        [WebMethod]
        public void DeleteMedication(int medicationId)
        {
            deleteMedication(medicationId);
        }       

        /// <summary>
        /// Web method called to save medication information
        /// </summary>
        /// <param name="medication">the medication object to be saved</param>
        [WebMethod]
        public void SaveMedication(Medication medication)
        {
            saveMedication(medication);
        }
       
        /// <summary>
        /// Gets all patients from database
        /// </summary>
        /// <returns></returns>
        private List<Patient> getPatients()
        {
            Patient patient;
            List<Patient> patients = new List<Patient>();
            SqlDataReader dataReader;
            SqlCommand cm;
            SqlConnection cn;
            //TODO all sql stuff
            string sql = "SELECT * FROM tblPatients";
            cn = new SqlConnection(connectionString);
            cn.Open();
            cm = new SqlCommand(sql, cn);
            dataReader = cm.ExecuteReader();
            while (dataReader.Read())
            {
                patient = new Patient();
                patient.fName = dataReader["fName"] as string;
                patient.lName = dataReader["lName"] as string;
                patient.patientId = dataReader["patientId"] as int?;
                patient.phoneNumber = dataReader["phoneNumber"] as int?;
                patients.Add(patient);
            }
            dataReader.Close();
            cm.Dispose();
            cn.Close();
            return patients;
        }

        /// <summary>
        /// Deletes patient form database based on patientId
        /// </summary>
        /// <param name="patientId">Id to be deleted</param>
        private void deletePatient(int patientId)
        {
            SqlCommand cm;
            SqlConnection cn;
            //TODO
            string sql = "DELETE FROM tblPatients WHERE patientId = @patientId";
            cn = new SqlConnection(connectionString);
            cn.Open();
            cm = new SqlCommand(sql, cn);
            cm.Parameters.Add("@patientId", SqlDbType.Int).Value = patientId;
            cm.ExecuteNonQuery();
            cm.Dispose();
            cn.Close();
        }

        /// <summary>
        /// Saves or inserts patient based on the id number passed
        /// </summary>
        /// <param name="patient">patient object to be saved or updated</param>
        private void savePatient(Patient patient)
        {
            SqlCommand cm;
            SqlConnection cn;
            string saveSql = "UPDATE tblPatients " +
                "SET fName=@fName, lName=@lName, phoneNumber=@phoneNumber, nextAppointment=@nextAppointment" +
                "WHERE patientId = @patientId";
            string insertSql = "INSERT INTO tblPatients (fName, lName, phoneNumber, nextAppointment)" +
                "VALUES (@fName, @lName, @phoneNumber, @nextAppointmenet)";

            cn = new SqlConnection(connectionString);
            cn.Open();
            if (patient.patientId == 0)
            {
                cm = new SqlCommand(insertSql, cn);
            }
            else
            {
                cm = new SqlCommand(saveSql, cn);
            }
            cm.Parameters.Add("@fName", SqlDbType.VarChar).Value = patient.fName;
            cm.Parameters.Add("@lName", SqlDbType.VarChar).Value = patient.lName;
            cm.Parameters.Add("@phoneNumber", SqlDbType.Int).Value = patient.phoneNumber;
            cm.Parameters.Add("@nextAppointment", SqlDbType.VarChar).Value = patient.nextAppointment;

            cm.ExecuteNonQuery();
            cm.Dispose();
            cn.Close(); 
        }

        /// <summary>
        /// Gets all appointments for a patient from database
        /// </summary>
        /// <param name="patientId">id of patient to get appoinments for</param>
        /// <returns>list of all appointments for a patient</returns>
        private List<Appointment> getAppointments(int patientId)
        {
            Appointment appointment;
            List<Appointment> appointments = new List<Appointment>();
            SqlDataReader dataReader;
            SqlCommand cm;
            SqlConnection cn;
            string sql = "SELECT * FROM tblAppointments WHERE patientId = @patientId";

            cn = new SqlConnection(connectionString);
            cn.Open();
            cm = new SqlCommand(sql, cn);
            cm.Parameters.Add("@patientId", SqlDbType.Int).Value = patientId;
            dataReader = cm.ExecuteReader();

            while (dataReader.Read())
            {
                appointment = new Appointment();
                appointment.appointmentId = dataReader["appointmentId"] as int?;
                appointment.appointmentCurrentMeds = dataReader["appointmentCurrentMeds"] as string;
                appointment.appointmentDate = dataReader["appointmentDate"] as string;
                appointment.appointmentDoctor = dataReader["appointmentDoctor"] as string;
                appointment.appointmentNewMedications = dataReader["appointmentNewMedications"] as string;
                appointment.appointmentNotes = dataReader["appointmentNotes"] as string;
                appointments.Add(appointment);
            }
            dataReader.Close();
            cm.Dispose();
            cn.Close();
            return appointments;
        }

        /// <summary>
        /// Deletes an appointment from the database based on the appointment id
        /// </summary>
        /// <param name="appointmentId">id to be deleted</param>
        private void deleteAppointment(int appointmentId)
        {
            SqlCommand cm;
            SqlConnection cn;
            //TODO
            string sql = "DELETE FROM tblAppointments WHERE appointmentId = @appointmentId";
            cn = new SqlConnection(connectionString);
            cn.Open();
            cm = new SqlCommand(sql, cn);
            cm.Parameters.Add("@appointmentId", SqlDbType.Int).Value = appointmentId;
            cm.ExecuteNonQuery();
            cm.Dispose();
            cn.Close();
        }

        /// <summary>
        /// Saves or inserts appointment based on the id number passed
        /// </summary>
        /// <param name="appointment">appointment object to be saved or updated</param>
        private void saveAppointment(Appointment appointment)
        {
            SqlCommand cm;
            SqlConnection cn;
            string saveSql = "UPDATE tblAppointments " +
                "SET appointmentCurrentMeds=@appointmentCurrentMeds, " +
                "appointmentDate=@appointmentDate, appointmentDoctor=@appointmentDoctor " +
                "appointmentNewMedications=@appointmentNewMedications, appointmentNotes=@appointmentNotes" +
                "WHERE appointmentId = @appointmentId";

            string insertSql = "INSERT INTO tblAppointments (appointmentCurrentMeds, " +
                "appointmentDate, appointmentDoctor, appointmentNewMedications, appointmentNotes)" +
                "VALUES ( @appointmentCurrentMeds, " +
                "@appointmentDate, @appointmentDoctor, @appointmentNewMedications, " +
                "@appointmentNotes)";

            cn = new SqlConnection(connectionString);
            cn.Open();
            if (appointment.appointmentId == 0)
            {
                cm = new SqlCommand(insertSql, cn);
            }
            else
            {
                cm = new SqlCommand(saveSql, cn);
            }
            cm.Parameters.Add("@appointmentCurrentMeds", SqlDbType.VarChar).Value = appointment.appointmentCurrentMeds;
            cm.Parameters.Add("@appointmentDate", SqlDbType.VarChar).Value = appointment.appointmentDate;
            cm.Parameters.Add("@appointmentDoctor", SqlDbType.Int).Value = appointment.appointmentDoctor;
            cm.Parameters.Add("@appointmentNewMedications", SqlDbType.VarChar).Value = appointment.appointmentNewMedications;
            cm.Parameters.Add("@appointmentNotes", SqlDbType.VarChar).Value = appointment.appointmentNotes;

            cm.ExecuteNonQuery();
            cm.Dispose();
            cn.Close();
        }

        /// <summary>
        /// get all medications for a given patient
        /// </summary>
        /// <param name="patientId">id of patient to get medicaitons for</param>
        /// <returns>List of all medications for a patient</returns>
        private List<Medication> getMedications(int patientId)
        {
            Medication medication;
            List<Medication> medications = new List<Medication>();
            SqlDataReader dataReader;
            SqlCommand cm;
            SqlConnection cn;
            string sql = "SELECT * FROM tblMedications WHERE patientId = @patientId";

            cn = new SqlConnection(connectionString);
            cn.Open();
            cm = new SqlCommand(sql, cn);
            cm.Parameters.Add("@patientId", SqlDbType.Int).Value = patientId;
            dataReader = cm.ExecuteReader();

            while (dataReader.Read())
            {
                medication = new Medication();
                medication.medicationActive = dataReader["medicationActive"] as bool?;
                medication.medicationDosage = dataReader["medicationDosage"] as string;
                medication.medicationId = dataReader["medicationId"] as int?;
                medication.medicationNotes = dataReader["medicationNotes"] as string;
                medication.medicationPerscriptionDate = dataReader["medicationPerscriptionDate"] as string;
                medication.medicationPerscriptionLength = dataReader["medicationPerscriptionLength"] as string;
                medications.Add(medication);
            }
            dataReader.Close();
            cm.Dispose();
            cn.Close();
            return medications;
        }

        /// <summary>
        /// deletes medication from database
        /// </summary>
        /// <param name="medicationId">id of medication to delete</param>
        private void deleteMedication(int medicationId)
        {
            SqlCommand cm;
            SqlConnection cn;
            //TODO
            string sql = "DELETE FROM tblAppointments WHERE medicationId = @medicationId";
            cn = new SqlConnection(connectionString);
            cn.Open();
            cm = new SqlCommand(sql, cn);
            cm.Parameters.Add("@appointmentId", SqlDbType.Int).Value = medicationId;
            cm.ExecuteNonQuery();
            cm.Dispose();
            cn.Close();
        }

        /// <summary>
        /// Saves or inserts medication based on the id number passed
        /// </summary>
        /// <param name="medication">the medication object to be saved</param>
        private void saveMedication(Medication medication)
        {
            SqlCommand cm;
            SqlConnection cn;
            string saveSql = "UPDATE tblMedications " +
                "SET medicationActive=@medicationActive, " +
                "medicationDosage=@medicationDosage, medicationNotes=@medicationNotes " +
                "medicationPerscriptionDate=@medicationPerscriptionDate, medicationPerscriptionLength=@medicationPerscriptionLength" +
                "WHERE appointmentId = @appointmentId";

            string insertSql = "INSERT INTO tblMedications (medicationActive, medicationDosage, " +
                "medicationNotes, medicationPerscriptionDate, medicationPerscriptionLength)" +
                "VALUES ( @medicationActive, " +
                "@medicationDosage, @medicationNotes, @medicationPerscriptionDate, " +
                "@medicationPerscriptionLength)";

            cn = new SqlConnection(connectionString);
            cn.Open();
            if (medication.medicationId == 0)
            {
                cm = new SqlCommand(insertSql, cn);
            }
            else
            {
                cm = new SqlCommand(saveSql, cn);
            }
            cm.Parameters.Add("@medicationActive", SqlDbType.Bit).Value = medication.medicationActive;
            cm.Parameters.Add("@medicationDosage", SqlDbType.VarChar).Value = medication.medicationDosage;
            cm.Parameters.Add("@medicationNotes", SqlDbType.VarChar).Value = medication.medicationNotes;
            cm.Parameters.Add("@medicationPerscriptionDate", SqlDbType.VarChar).Value = medication.medicationPerscriptionDate;
            cm.Parameters.Add("@medicationPerscriptionLength", SqlDbType.VarChar).Value = medication.medicationPerscriptionLength;
            cm.ExecuteNonQuery();
            cm.Dispose();
            cn.Close();
        }
    }
}