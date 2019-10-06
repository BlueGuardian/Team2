<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientsPage.aspx.cs" Inherits="ProjectTeam2.PatientsPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Paitients Page</title>
</head>
<body>
    <header>Patients Page</header>
        <div id ="divTables">
            <div id ="divMain">
            <table id ="tblMain">
            </table>
                </div>
            <div id ="divAppointments"style ="visibility: hidden">
            <table id ="tblAppointments" ></table>
                </div>
            <div id ="divMedications"style ="visibility: hidden">
            <table id ="tblMedications" ></table>
                </div>
        </div>
    <div id ="divPopUp" style ="visibility: hidden">
        <a id="btnClosePopUp">Close</a>
    </div>
</body>
</html>


<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/v/dt/dt-1.10.18/datatables.min.js"></script>

<script type ="text/javascript">

    var tblMain;
    var tblAppointments;
    var tblMedications;
    var divPopUp = $('#divPopUp');

    //closes the patient pop up window
    $('#btnClosePopUp').on('click', function (e) {
        divPopUp.hide();
    });

    //Script that runs once the page is loaded
    $(document).ready(function () {
        //Main table creation
        tblMain = $('#tblMain').DataTable({
            'lengthMenu': [[10, 20, 50], [10, 20, 50]],
            'dom': 'Blfrtip',
            'buttons': [{
                'text': "Add Patient",
                'action': function () {
                    addPatient();
                }
            }],
            'paging': false,
            'columns': [
                { "data": "fName", "title": "First Name" },
                { "data": "lName", "title": "Last Name" },
                { "data": "phoneNumber", "title": "Phone Number" },
                { "data": "nextAppointment", "title": "Next Appointment" },
                { "data": "patientId", "title": "", "visible": false },
                {
                    "title": "Appointments",
                    "render": function (data, type, row, meta) {
                        return "<a onclick='javascript:switchTotblAppointments(" + patientId + ");'>Appointments</a>}"
                    }
                },
                {
                    "title": "Medications",
                    "render": function (data, type, row, meta) {
                        return "<a onclick='javascript:switchTotblMedications(" + patientId + ");'>Medications</a>}"
                    }
                },
                {
                    "title": "Delete",
                    "render": function (data, type, row, meta) {
                        return "<a onclick='javascript:deletePatient(" + appointmentId + ");'>Delete</a>}"
                    }
                },
                {
                    "title": "Edit",
                    "render": function (data, type, row, meta) {
                        return "<a onclick='javascript:editPatient(" + appointmentId + ");'>Edit</a>}"
                    }
                }

            ]
        });

        //Appointment table creation
        tblAppointments = $('#divtblAppointments').DataTable({
            'lengthMenu': [[10, 20, 50], [10, 20, 50]],
            'dom': 'Blfrtip',
            'buttons': [{
                'text': "Add Appointment",
                'action': function () {
                    addAppointment();
                }
            },
                {
                    'text': "Back to main",
                    'action': function () {
                        switchToTblMain();
                    }
                }
            ],
            'paging': false,
            'columns': [
                { "data": "appointmentDate", "title": "Date" },
                { "data": "appointmentDoctor", "title": "Doctar" },
                { "data": "appointmentCurrentMeds", "title": "Current Medications" },
                { "data": "appointmentNewMedications", "title": "Perscribed Medications" },
                { "data": "appointmentNotes", "title": "Notes" },
                { "data": "appointmentId", "title": "", "visible": false},
                {
                    "title": "Delete",
                    "render": function (data, type, row, meta) {
                        return "<a onclick='javascript:deleteAppointment(" + appointmentId + ");'>Delete</a>}"
                    }
                },
                {
                    "title": "Edit",
                    "render": function (data, type, row, meta) {
                        return "<a onclick='javascript:editAppointment(" + appointmentId + ");'>Edit</a>}"
                    }
                }
            ]
        });

        //Medications table creation 
        tblMedications = $('#divtblMedications').DataTable({
            'lengthMenu': [[10, 20, 50], [10, 20, 50]],
            'dom': 'Blfrtip',
            'buttons': [{
                'text': "Add Medication",
                'action': function () {
                    addMedications();
                }
            },
                {
                    'text': "Back to main",
                    'action': function () {
                        switchToTblMain();
                    }
                }
            ],
            'paging': false,
            'columns': [
                { "data": "medicationPerscriptionDate", "title": "Date" },
                { "data": "medicationPerscriptionLength", "title": "Length" },
                { "data": "medicationDosage", "title": "Dosage" },
                { "data": "medicationActive", "title": "Currently Taken" },
                { "data": "medicationNotes", "title": "Notes" },
                { "data": "medicationId", "title": "", "visible": false},
                {
                    "title": "Delete",
                    "render": function (data, type, row, meta) {
                        return "<a onclick='javascript:deleteMedication(" + appointmentId + ");'>Delete</a>}"
                    }
                },
                {
                    "title": "Edit",
                    "render": function (data, type, row, meta) {
                        return "<a onclick='javascript:editMedication(" + appointmentId + ");'>Edit</a>}"
                    }
                }

            ]
        });

    });

    //switchs to appointment table
    function switchTotblAppointments(patientId) {
        $("divMain").hide();
        loadAppointmentsTable(patientId);
        $("#divAppointments").show();
    };

    //switches to medications table
    function switchTotblMedications(patientId) {
        $("divMain").hide();
        loadMedicationsTable(patientId);
        $("divMedications").show();
    };

    //switchs to main table
    function switchToTblMain() {
        $("#divAppointments").hide();
         $("divMedications").hide();
        $("divMain").show();
    };

    //Loads data into main table
    function loadMainTable() {
        $.ajax({
            dataType: 'json',
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: ResolveUrl("~/PatientsPage.aspx/LoadMainTable"),
            success: function (msg) {
                var data = msg.objData;
                tblMain.rows().add(data).draw();
            },
            error: function () {
                tblMain.rows().clear();
            },
            complete: function () {

            }
        });
    }

    //Edits patient
    function editPatient(patientId) {
         //TODO
        //show patient modal
        //Check the patient id to see if its an edit or new
        if (patientId != null) {
            //Load appointment info to modal
        }
        else {
            //open blank appointment modal
        }
        savePatient(patientId);
    }

    //deletes patient
    function deletePatient(patientId) {
        //TODO popup to double check they want this deleted
         $.ajax({
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: ResolveUrl("~/PatientsPage.aspx/DeletePatient"),
            data: JSON.stringify({ patientId: patientId }),
            success: function (msg) {
                //TODO Show pop up to show it was deleted
                
            },
            error: function () {
            },
            complete: function () {

            }
        });
    }

    //Saves a patients information
    function savePatient(patient) {
        $.ajax({
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: ResolveUrl("~/PatientsPage.aspx/SaveAppointment"),
            data: JSON.stringify({ patient: patient }),
            success: function (msg) {
                //TODO Show pop up to show it was saved                
            },
            error: function () {
            },
            complete: function () {

            }
        });
    }

    //loads data into appointments table
    function loadAppointmentsTable(patientId) {
        $.ajax({
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: ResolveUrl("~/PatientsPage.aspx/LoadAppointmentsTable"),
            data: JSON.stringify({ patientId: patientId }),
            success: function (msg) {
                var data = msg.objData;
                tblAppointments.rows().add(data).draw();
            },
            error: function () {
                tblAppointments.rows().clear();
            },
            complete: function () {

            }
        });
    }

    //deletes appointment from table
    function deleteAppointmnet(appointmentId) {
        //TODO popup to double check they want this deleted
         $.ajax({
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: ResolveUrl("~/PatientsPage.aspx/DeleteAppointment"),
            data: JSON.stringify({ appointmentId: appointmentId }),
            success: function (msg) {
                //TODO Show pop up to show it was deleted
                
            },
            error: function () {
            },
            complete: function () {

            }
        });
    }

    //Editing/adding appointment logic
    function editAppointment(appointmentId) {
         //TODO
        //show appointment modal
        //Check the appointment id to see if its an edit or new
        if (medicaionId != null) {
            //Load appointment info to modal
        }
        else {
            //open blank appointment modal
        }
        saveAppointment(appointmentId);
    }

    //Save appointment
    function saveAppointment(appointment) {
         $.ajax({
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: ResolveUrl("~/PatientsPage.aspx/SaveAppointment"),
            data: JSON.stringify({ appointment: appointment }),
            success: function (msg) {
                //TODO Show pop up to show it was saved
                
            },
            error: function () {
            },
            complete: function () {

            }
        });
    }
    //loads data into medications table
    function loadMedicationsTable(patientId) {
        $.ajax({
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: ResolveUrl("~/PatientsPage.aspx/LoadMedicationsTable"),
            data: JSON.stringify({ patientId: patientId }),
            success: function (msg) {
                var data = msg.objData;
                tblMedications.rows().add(data).draw();
            },
            error: function () {
                tblMedications.rows().clear();
            },
            complete: function () {

            }
        });
    }

    //deletes medication from table
    function deleteMedication(medicationId) {
        //TODO popup to double check they want this deleted
         $.ajax({
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: ResolveUrl("~/PatientsPage.aspx/DeleteMedication"),
            data: JSON.stringify({ medicationId: medicationId }),
            success: function (msg) {
                //TODO Show pop up to show it was delete
                
            },
            error: function () {
            },
            complete: function () {

            }
        });
    }

    //Editing/adding medication logic
    function editMedication(medicationId) {
         //TODO
        //show medication modal
        //Check the medicaiton id to see if its an edit or new
        if (medicaionId != null) {
            //Load medication info to modal
        }
        else {
            //open blank medication modal
        }
        saveMedication(medicationId);
    }

    //Save medication
    function saveMedication(medication) {
         $.ajax({
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: ResolveUrl("~/PatientsPage.aspx/SaveMedication"),
            data: JSON.stringify({ medication: medication }),
            success: function (msg) {
                //TODO Show pop up to show it was saved
                
            },
            error: function () {
            },
            complete: function () {

            }
        });
    }
    //TODO
    //Create popup window
    //Add edit, add, and delete buttons. 
</script>