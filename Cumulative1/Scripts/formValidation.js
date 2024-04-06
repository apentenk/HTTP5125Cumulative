function validateInputData(input) {
    //valid if Empty
    if (input.value === "" || input.value === null) {
        return invalidData(input);
    } else {
        return validData(input);
    }
}

function validateInputDataRegex(input, regex) {
    //invalid if input string does not match regex
    if (regex.test(input.value)) {
        return validData(input);
    } else {
        return invalidData(input);
    }
}

function invalidData(input) {
    //If invalid change border to red and reveal error message
    input.classList.add("inputError");
    for (label of input.labels) {
        if (label.classList.contains("error")) {
            label.style.display = "initial";
        }
    }
    input.focus();
    return false;
}

function validData(input) {
    //If valid change border back to black and hide error message
    input.classList.remove("inputError");
    for (label of input.labels) {
        if (label.classList.contains("error")) {
            label.style.display = "none";
        }
    }
    return true;
}