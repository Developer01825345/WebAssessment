document.addEventListener("DOMContentLoaded", async () => {
    const userId = window.location.pathname.split("/").pop();
    const response = await fetch(`http://localhost:5157/api/Users/${userId}`);

    if (response.ok) {
        const userData = await response.json();
        document.getElementById("userid").value = userData.userId;
        document.getElementById("firstname").value = userData.firstName;
        document.getElementById("lastname").value = userData.lastName;
        document.getElementById("email").value = userData.email;
        document.getElementById("phone").value = userData.phoneNumber;
        document.getElementById("address").value = userData.address;
    }
});

document.getElementById("editForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    let firstName = $("#firstname").val().trim();
    let lastName = $("#lastname").val().trim();
    const phone = document.getElementById("phone").value.trim();
    const email = document.getElementById("email").value.trim();
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

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

    await updateUser();
});

async function updateUser() {
    const user = {
        UserId: document.getElementById("userid").value,
        FirstName: document.getElementById("firstname").value.trim(),
        LastName: document.getElementById("lastname").value.trim(),
        Email: document.getElementById("email").value.trim(),
        PhoneNumber: document.getElementById("phone").value.trim(),
        Address: document.getElementById("address").value.trim()
    };

    const response = await fetch(`http://localhost:5157/api/Users/${user.UserId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
    });

    const data = await response.json();
    
    if (response.ok) {
        showMessage(data.message, "success");
    } else {
        showMessage(data.message, "danger");
    }
}

function showMessage(message, type) {
    $("#message").html(`<div class="alert alert-${type}">${message}</div>`);
}