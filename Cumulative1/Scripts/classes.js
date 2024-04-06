function AddCourse() {
    var URL = "http://localhost:49934/api/ClassData/AddClass/";

    var xhr = new XMLHttpRequest();

    //Getting Input Data from the form
    var ClassCode = document.getElementById('ClassCode');
    var ClassName = document.getElementById('ClassName');
    var TeacherID = document.getElementById('TeacherID');
    var StartDate = document.getElementById('StartDate');
    var FinishDate = document.getElementById('FinishDate');

    //Validating the data
    var valid = ValidateClassForm(ClassCode, ClassName, TeacherID, StartDate, FinishDate);

    if (valid === 0) {
        return;
    }

    //Mapping data to object
    var CourseData = {
        "ClassCode": ClassCode.value,
        "ClassName": ClassName.value,
        "TeacherID": TeacherID.value,
        "StartDate": StartDate.value,
        "FinishDate": FinishDate.value
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
    xhr.send(JSON.stringify(CourseData));
}

function DeleteCourse(id) {
    var URL = "http://localhost:49934/api/ClassData/DeleteClass/" + id;

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

function ValidateClassForm(ClassCode, ClassName) {
    //Determining that there is no invalid data
    return validateInputData(FinishDate) & validateInputData(StartDate) & validateInputData(TeacherID) & validateInputData(ClassName) & validateInputData(ClassCode);
}