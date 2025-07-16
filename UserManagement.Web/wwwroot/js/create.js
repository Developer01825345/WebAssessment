$(document).ready(function () {
    $("#createForm").on("submit", async function (e) {
        e.preventDefault();

        let firstName = $("#firstname").val().trim();
        let lastName = $("#lastname").val().trim();
        let phone = $("#phone").val().trim();
        let email = $("#email").val().trim();
        let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!firstName) {
            showMessage("First Name is required", "danger");
            return;
        }

        if (!lastName) {
            showMessage("Last Name is required", "danger");
            return;
        }
        
        if (!emailPattern.test(email)) {
            showMessage("Invalid email format", "danger");
            return;
        }

        if (phone.length < 10) {
            showMessage("Phone number must be at least 10 digits", "danger");
            return;
        }

        await saveUser();
    });
});

async function saveUser() {
    const user = {
        firstName: $("#firstname").val().trim(),
        lastName: $("#lastname").val().trim(),
        email: $("#email").val().trim(),
        phoneNumber: $("#phone").val().trim(),
        address: $("#address").val().trim()
    };

    const response = await fetch("http://localhost:5157/api/Users", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
    });

    const data = await response.json();

    if (response.ok) {
        showMessage(data.message, "success");
        $("#createForm")[0].reset();
    } else {
        showMessage(data.message, "danger");
    }
}

function showMessage(message, type) {
    $("#message").html(`<div class="alert alert-${type}">${message}</div>`);
}