window.dashboardService = (function () {

    function updateDashboard(employees) {

        if (!Array.isArray(employees)) {
            console.warn("Dashboard: invalid employees data");
            employees = [];
        }

        // Total Employees
        document.getElementById("totalEmployees").innerText = employees.length;

        // Departments (unique count)
        const departments = new Set();

        employees.forEach(emp => {
            if (emp.department) {
                departments.add(emp.department);
            }
        });

        document.getElementById("totalDepartments").innerText = departments.size;

        // Active users (static for now)
        document.getElementById("activeUsers").innerText = 1;
    }

    return {
        updateDashboard
    };

})();