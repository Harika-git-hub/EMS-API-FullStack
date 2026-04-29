window.uiService = (function () {

    function showSection(sectionId) {

        document.querySelectorAll(".content-view, #authSection").forEach(sec => {
            sec.classList.add("d-none");
        });

        const section = document.getElementById(sectionId);
        if (section) {
            section.classList.remove("d-none");
        }

        if (sectionId === "employeesSection") {
            loadEmployees();
        }

        if (sectionId === "dashboardSection") {
            loadEmployees(); // refresh dashboard too
        }
    }

    async function loadEmployees() {
        try {

            console.log("Loading employees...");

            const employees = await window.employeeService.getAll();

            renderEmployeeTable(employees);

            // 🔥 UPDATE DASHBOARD COUNTS
            window.dashboardService.updateDashboard(employees);

        } catch (err) {
            console.error("Error loading employees:", err);
        }
    }

    function renderEmployeeTable(employees) {

        const tbody = document.querySelector("#employeeTable tbody");

        if (!tbody) return;

        tbody.innerHTML = "";

        if (!Array.isArray(employees) || employees.length === 0) {
            tbody.innerHTML = `
                <tr>
                    <td colspan="5" class="text-center text-muted">
                        No employees found
                    </td>
                </tr>
            `;
            return;
        }

        employees.forEach(emp => {

            const row = document.createElement("tr");

            row.innerHTML = `
                <td>${emp.firstName || ""} ${emp.lastName || ""}</td>
                <td>${emp.email || ""}</td>
                <td>${emp.department || ""}</td>
                <td>${emp.salary || ""}</td>
                <td>${emp.status || ""}</td>
                <td>
                    <button class="btn btn-sm btn-danger">Delete</button>
                </td>
            `;

            tbody.appendChild(row);
        });
    }

    return {
        showSection,
        loadEmployees,
        renderEmployeeTable
    };

})();