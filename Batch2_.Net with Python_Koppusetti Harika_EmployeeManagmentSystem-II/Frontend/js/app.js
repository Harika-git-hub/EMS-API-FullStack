document.addEventListener("DOMContentLoaded", function () {

    console.log("App started");

    // ================= LOGIN =================
    const loginForm = document.getElementById("loginForm");
    loginForm?.addEventListener("submit", async function (e) {
        e.preventDefault();
        const username = document.getElementById("loginUsername").value;
        const password = document.getElementById("loginPassword").value;
        const result = await authService.login(username, password);
        if (result.success) {
            document.getElementById("authSection").classList.add("d-none");
            document.getElementById("mainNavbar").classList.remove("d-none");
            document.getElementById("currentUserName").innerText = username;
            uiService.showSection("dashboardSection");
        }
    });

    // ================= SIGNUP TOGGLE =================
    document.getElementById("createAccountLink")?.addEventListener("click", function (e) {
        e.preventDefault();
        document.getElementById("loginView").classList.add("d-none");
        document.getElementById("signupView").classList.remove("d-none");
    });

    document.getElementById("switchToLoginLink")?.addEventListener("click", function (e) {
        e.preventDefault();
        document.getElementById("signupView").classList.add("d-none");
        document.getElementById("loginView").classList.remove("d-none");
    });

    // ================= SIGNUP =================
    const signupForm = document.getElementById("signupForm");
    signupForm?.addEventListener("submit", async function (e) {
        e.preventDefault();
        const username = document.getElementById("signupUsername").value;
        const password = document.getElementById("signupPassword").value;
        const confirmPassword = document.getElementById("confirmPassword").value;
        
        if (password!== confirmPassword) {
            alert("Passwords do not match");
            return;
        }
        
        const result = await authService.signup(username, password);
        if (result.success) {
            alert("Account created! Please login.");
            document.getElementById("signupView").classList.add("d-none");
            document.getElementById("loginView").classList.remove("d-none");
        }
    });

    // ================= NAVIGATION =================
    document.addEventListener("click", function (e) {
        const link = e.target.closest("[data-view]");
        if (!link) return;
        e.preventDefault();
        const view = link.getAttribute("data-view");
        
        document.querySelectorAll('.nav-link').forEach(nav => nav.classList.remove('active'));
        link.classList.add('active');
        
        uiService.showSection(view);
        if (view === 'employeesSection') {
            uiService.loadEmployees();
        }
    });

    // ================= LOGOUT =================
    document.getElementById("logoutBtn")?.addEventListener("click", function () {
        authService.logout();
        document.getElementById("mainNavbar").classList.add("d-none");
        document.getElementById("dashboardSection").classList.add("d-none");
        document.getElementById("employeesSection").classList.add("d-none");
        document.getElementById("authSection").classList.remove("d-none");
    });

});

// ================= EMPLOYEE MODAL =================
document.addEventListener('click', function(e) {
    
    // 1. ADD EMPLOYEE BUTTON CLICK
    if (e.target.closest('#addEmployeeBtn')) {
        console.log('Add Employee clicked');
        const form = document.getElementById('employeeForm');
        const modalEl = document.getElementById('employeeModal');
        
        if (!modalEl) {
            console.error('Modal #employeeModal not found');
            return;
        }
        
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        form.reset();
        document.getElementById('empJoinDate').valueAsDate = new Date();
        
        const title = document.querySelector('#employeeModal.modal-title');
        if (title) title.innerText = 'Add Employee';
        
        modal.show();
    }

    // 2. SAVE EMPLOYEE BUTTON CLICK 
    if (e.target.closest('#saveEmployeeBtn')) {
        const saveBtn = e.target.closest('#saveEmployeeBtn');
        const fullName = document.getElementById('empName').value.trim();
        const nameParts = fullName.split(' ');
        
        // FIX: Send PascalCase for C# API + include Name field for SQL
        const employeeData = {
            Name: fullName,
            FirstName: nameParts[0] || '',
            LastName: nameParts.slice(1).join(' ') || 'N/A',
            Email: document.getElementById('empEmail').value.trim(),
            Phone: document.getElementById('empPhone').value.trim(),
            Department: document.getElementById('empDept').value,
            Designation: document.getElementById('empDesignation').value.trim(),
            Salary: parseFloat(document.getElementById('empSalary').value),
            JoinDate: new Date(document.getElementById('empJoinDate').value).toISOString(),
            Status: "Active"
        };

        if (!employeeData.Name ||!employeeData.Email ||!employeeData.Phone || 
        !employeeData.Department ||!employeeData.Designation ||!employeeData.Salary ||!employeeData.JoinDate) {
            alert('Please fill all fields');
            return;
        }

        saveBtn.disabled = true;
        saveBtn.innerText = 'Saving...';

        employeeService.addEmployee(employeeData)
    .then(() => {
                bootstrap.Modal.getInstance(document.getElementById('employeeModal')).hide();
                uiService.loadEmployees();
            })
    .catch(err => {
                console.error('Add employee failed:', err);
                alert('Error: ' + err.message);
            })
    .finally(() => {
                saveBtn.disabled = false;
                saveBtn.innerText = 'Save';
            });
    }
});