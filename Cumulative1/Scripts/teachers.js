//Employee Number Format
const EMPLOYEE_NUMBER_REGEX = /T[0-9]+/;
//Salary Number Format
const SALARY_REGEX = /[0-9]+\.[0-9]{2}/;
function AddTeacher() {
    var URL = "http://localhost:49934/api/TeacherData/AddTeacher/";

    var xhr = new XMLHttpRequest();

    //Getting Input Data from the form
    var TeacherFname = document.getElementById('TeacherFname');
    var TeacherLname = document.getElementById('TeacherLname');
    var EmployeeNumber = document.getElementById('EmployeeNumber');
    var HireDate = document.getElementById('HireDate');
    var Salary = document.getElementById('Salary');

    //Validating the data
    var valid = ValidateTeacherForm(TeacherFname, TeacherLname, EmployeeNumber, HireDate, Salary);

    if (valid === 0) {
        return;
    }

    //Mapping data to object
    var StudentData = {
        "FirstName": TeacherFname.value,
        "LastName": TeacherLname.value,
        "EmployeeNumber": EmployeeNumber.value,
        "HireDate": HireDate.value,
        "Salary": Salary.value
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

function DeleteTeacher(id) {
    var URL = "http://localhost:49934/api/TeacherData/DeleteTeacher/" + id;

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

function UpdateTeacher(id) {
    var URL = "http://localhost:49934/api/TeacherData/UpdateTeacher/" + id;

    var xhr = new XMLHttpRequest();

    //Getting Input Data from the form
    var TeacherFname = document.getElementById('TeacherFname');
    var TeacherLname = document.getElementById('TeacherLname');
    var EmployeeNumber = document.getElementById('EmployeeNumber');
    var HireDate = document.getElementById('HireDate');
    var Salary = document.getElementById('Salary');

    //Validating the data
    var valid = ValidateTeacherForm(TeacherFname, TeacherLname, EmployeeNumber, HireDate, Salary);

    if (valid === 0) {
        return;
    }

    //Mapping data to object
    var TeacherData = {
        "FirstName": TeacherFname.value,
        "LastName": TeacherLname.value,
        "EmployeeNumber": EmployeeNumber.value,
        "HireDate": HireDate.value,
        "Salary": Salary.value
    };

    //Generating UPDATE request
    xhr.open("POST", URL, true);
    xhr.setRequestHeader("Content-Type", "application/json");

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 204) {
            window.location.replace("../Show/" + id);
        }
    }
    xhr.send(JSON.stringify(TeacherData));
}

function ValidateTeacherForm(TeacherFName, TeacherLname, EmployeeNumber, HireDate, Salary) {
    //Determining that there is no invalid data
    //Bitwise AND so that there is not short circuit evalution and all fields are validated
    return validateInputDataRegex(Salary, SALARY_REGEX) & validateInputData(HireDate) & validateInputDataRegex(EmployeeNumber, EMPLOYEE_NUMBER_REGEX) & validateInputData(TeacherLname) & validateInputData(TeacherFName);
}

