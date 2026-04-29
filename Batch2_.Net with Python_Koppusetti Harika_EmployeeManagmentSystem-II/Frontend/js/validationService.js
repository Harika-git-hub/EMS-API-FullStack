(function (root, factory) {
  if (typeof module !== 'undefined' && module.exports) {
    module.exports = factory(require('./employeeService'), require('./storageService'));
  } else {
    root.validationService = factory(root.employeeService, root.storageService);
  }
})(typeof window !== 'undefined' ? window : globalThis, function (employeeService, storageService) {
  const departments = ['Software Engineering','Marketing & Branding','Sales & Business Development','Human Resources',
  'Product Development','UI/UX Design','Cyber Security','Customer Experience','Business Intelligence','Quality Assurance',
  'Research & Innovation','Finance & Accounts','Operations & Logistics','Learning & Development','Legal & Compliance'];
  const statuses = ['Active', 'Inactive'];

  /**
   * Validates login or signup form input.
   * @param {{username:string, password:string, confirmPassword?:string}} formData Auth payload.
   * @param {'login'|'signup'} mode Validation mode.
   * @returns {Object<string, string>} Field-level error map.
   */
  function validateAuthForm(formData, mode) {
    const errors = {};
    const username = String(formData.username || '').trim();
    const password = String(formData.password || '');
    const confirmPassword = String(formData.confirmPassword || '');

    if (!username) errors.username = 'Username is required.';
    if (!password) errors.password = 'Password is required.';

    if (mode === 'signup') {
      if (!confirmPassword) errors.confirmPassword = 'Confirm password is required.';
      if (password && password.length < 6) errors.password = 'Password must be at least 6 characters.';
      if (password && confirmPassword && password !== confirmPassword) {
        errors.confirmPassword = 'Passwords do not match.';
      }
      const admins = storageService?.getAdmins?.() || [];
      const exists = admins.some((admin) => admin.username.toLowerCase() === username.toLowerCase());
      if (username && exists) errors.username = 'Username already exists.';
    }

    return errors;
  }

  /**
   * Validates add/edit employee form input.
   * @param {Object} formData Employee payload.
   * @returns {Object<string, string>} Field-level error map.
   */
  function validateEmployeeForm(formData) {
    const errors = {};
    const firstName = String(formData.firstName || '').trim();
    const lastName = String(formData.lastName || '').trim();
    const email = String(formData.email || '').trim();
    const phone = String(formData.phone || '').trim();
    const department = String(formData.department || '').trim();
    const designation = String(formData.designation || '').trim();
    const salary = Number(formData.salary);
    const joinDate = String(formData.joinDate || '').trim();
    const status = String(formData.status || '').trim();

    // ✅ Required fields
    if (!firstName) errors.firstName = 'First name is required.';
    if (!lastName) errors.lastName = 'Last name is required.';
    if (!email) errors.email = 'Email is required.';
    if (!phone) errors.phone = 'Phone is required.';
    if (!department) errors.department = 'Department is required.';
    if (!designation) errors.designation = 'Designation is required.';
    if (!joinDate) errors.joinDate = 'Join date is required.';
    if (!status) errors.status = 'Status is required.';
    if (formData.salary === undefined || formData.salary === '') {
      errors.salary = 'Salary is required.';
    }

    // ✅ Name validation
    if (firstName && !/^[A-Za-z]+$/.test(firstName)) {
      errors.firstName = 'First name should contain only letters.';
    }

    if (lastName && !/^[A-Za-z]+$/.test(lastName)) {
      errors.lastName = 'Last name should contain only letters.';
    }

    // ✅ Email
    if (email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      errors.email = 'Enter a valid email address.';
    }

    if (email && employeeService.emailExists(email, formData.id)) {
      errors.email = 'Email already exists.';
    }

    // ✅ Phone
    if (phone && !/^\d{10}$/.test(phone)) {
      errors.phone = 'Phone must be exactly 10 digits.';
    }

    // ✅ Salary
    if (!isNaN(salary) && salary <= 0) {
      errors.salary = 'Salary must be a positive number.';
    }

    // ✅ Department
    if (department && !departments.includes(department)) {
      errors.department = 'Please select a valid department.';
    }

    // ✅ Status
    if (status && !statuses.includes(status)) {
      errors.status = 'Please select a valid status.';
    }

    // ✅ Date
    if (joinDate && !/^\d{4}-\d{2}-\d{2}$/.test(joinDate)) {
      errors.joinDate = 'Join Date must be in YYYY-MM-DD format.';
    }

    return errors;
  }

  return { validateEmployeeForm, validateAuthForm };
});