async function deleteUser(userId) {
    if (!confirm("Are you sure you want to delete this user?")) return;

    const response = await fetch(`http://localhost:5157/api/Users/${userId}`, {
        method: "DELETE"
    });

    if (response.ok) {
        alert("User deleted successfully!");
        location.reload();
    } else {
        alert("Error deleting user!");
    }
}