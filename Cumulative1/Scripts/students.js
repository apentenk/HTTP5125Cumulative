//Student Number Format
const STUDENT_NUMBER_REGEX = /N[0-9]+/
function AddStudent() {
    var URL = "http://localhost:49934/api/StudentData/AddStudent/";

    var xhr = new XMLHttpRequest();

    //Getting Input Data from the form
    var StudentFname = document.getElementById('StudentFname');
    var StudentLname = document.getElementById('StudentLname');
    var StudentNumber = document.getElementById('StudentNumber');
    var EnrollDate = document.getElementById('EnrollDate');

    //Validating the data
    var valid = ValidateStudentForm(StudentFname, StudentLname, StudentNumber, EnrollDate);

    if (valid === 0) {
        return;
    }

    //Mapping data to object
    var StudentData = {
        "FirstName": StudentFname.value,
        "LastName": StudentLname.value,
        "StudentNumber": StudentNumber.value,
        "EnrolDate": EnrollDate.value
    };

    //Generatiing POST request
    xhr.open("POST", URL, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 204) {
            //Redirecting after successful response
            window.location.replace("List");
        }
    }

    //Converting Object to JSON and sending as request payload
    xhr.send(JSON.stringify(StudentData));
}

function DeleteStudent(id) {
    var URL = "http://localhost:49934/api/StudentData/DeleteStudent/" + id;

    var xhr = new XMLHttpRequest();

    //Generating DELETE request
    xhr.open("DELETE", URL, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 204) {
            //Redirecting after successful response
            window.location.replace("../List");
        }
    }

    //Sending request
    xhr.send();
}

function ValidateStudentForm(StundentFname, StudentLname, StudentNumber, EnrollDate) {
    //Determining that there is no invalid data
    return validateInputData(EnrollDate) & validateInputDataRegex(StudentNumber, STUDENT_NUMBER_REGEX) & validateInputData(StudentLname) & validateInputData(StundentFname);
}