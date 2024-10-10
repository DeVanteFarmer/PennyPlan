const apiUrl = "https://localhost:5001";

export const login = (userObject) => {
  return fetch(`${apiUrl}/api/user/getbyemail?email=${userObject.email}`)
    .then((response) => response.json())
    .then((user) => {
      // Assuming the back-end checks the password
      if (user && user.password_hash === userObject.password_hash) {
        localStorage.setItem("user", JSON.stringify(user));
        return user;
      } else {
        return undefined;
      }
    });
};

export const logout = () => {
  localStorage.clear();
};

export const register = (userObject) => {
  return fetch(`${apiUrl}/api/user`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(userObject),
  })
    .then((response) => response.json())
    .then((savedUser) => {
      localStorage.setItem("user", JSON.stringify(savedUser));
    });
};

  