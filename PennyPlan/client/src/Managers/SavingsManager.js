const apiUrl = "https://localhost:5001/api/Savings";

export const getUserSavings = (userObject) => {
  return fetch(`${apiUrl}/user/${userObject.id}`).then((res) => res.json());
};


//fetch to add savings to database
export const addSavings = (savings) => {
  return fetch(apiUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(savings),
  });
};

//fetch to edit a savings
export const editSavings = (savings) => {
    return fetch(`${apiUrl}/${savings.id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(savings)
    })
}

// fetch to delete a Savings
export const deleteSavings = (id) => {
    return fetch(`${apiUrl}/${id}`, {
        method: "DELETE"
    });
};
