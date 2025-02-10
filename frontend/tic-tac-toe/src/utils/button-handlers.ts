export const makePasswordVisible = () => {
  const passwordInput = document.getElementById('password') as HTMLInputElement | null;
  const confirmInput = document.getElementById('confirm') as HTMLInputElement | null;
  const showPasswordButton = document.getElementById('showPasswordButton');

  if (passwordInput!.type === 'password') {
    passwordInput!.type = 'text';
    confirmInput!.type = 'text';
    showPasswordButton!.textContent = 'Скрыть пароль';
  } else {
    passwordInput!.type = 'password';
    confirmInput!.type = 'password';
    showPasswordButton!.textContent = 'Показать пароль';
  }
};
