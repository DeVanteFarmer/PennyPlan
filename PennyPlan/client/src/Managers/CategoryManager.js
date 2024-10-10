const apiUrl = "https://localhost:5001/api/Category";

// Fetch all categories
export const getCategories = () => {
  return fetch(apiUrl, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  }).then((response) => response.json());
};

// Fetch a category by its ID
export const getCategoryById = (id) => {
  return fetch(`${apiUrl}/${id}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  }).then((response) => response.json());
};

// Add a new category
export const addCategory = (category) => {
  return fetch(apiUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(category),
  }).then((response) => response.json());
};

// Update an existing category
export const updateCategory = (category) => {
  return fetch(`${apiUrl}/${category.id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(category),
  });
};

// Delete a category by its ID
export const deleteCategory = (id) => {
  return fetch(`${apiUrl}/${id}`, {
    method: "DELETE",
  });
};
