import errors from './error-messages.ts';

export const validatePassword = (password: string) => {
  if (password.length < 8 || password.length > 12) {
    return errors.passwordLength;
  }

  const hasUpperCase = /[A-Z]/.test(password);
  const hasLowerCase = /[a-z]/.test(password);
  const hasDigits = /\d/.test(password);
  const hasSpecialChars = /[!@#$%]/.test(password);

  if (!hasUpperCase || !hasLowerCase || !hasDigits || !hasSpecialChars) {
    if (!hasUpperCase) return errors.passwordUpperCase;
    if (!hasLowerCase) return errors.passwordLowerCase;
    if (!hasDigits) return errors.passwordDigits;
    if (!hasSpecialChars) return errors.passwordSpecialChars;
  }

  return '';
};

export const validateUsername = (username: string) => {
  if (!username) {
    return errors.usernameEmpty;
  }
  if (/[^a-zA-Z0-9_]/.test(username)) {
    return errors.usernameInvalid;
  }
  return '';
};

