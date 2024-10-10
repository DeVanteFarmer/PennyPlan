/* 
Key Points:
Categories come first because Bills and Transactions need valid CategoryId references.
Users are inserted next, as Bills, Savings, and Transactions need valid UserId references.
Bills, Savings, and Transactions come after the corresponding categories and users have been inserted, ensuring no foreign key conflicts.
*/ 

INSERT INTO Categories (Name, IsBillCategory, IsTransactionCategory)
VALUES 
('Rent', 1, 0),  
('Utilities', 1, 0),  
('Internet', 1, 0),  
('Phone Bill', 1, 0),  
('Health Insurance', 1, 0),  
('Car Payment', 1, 0),  
('Car Insurance', 1, 0),  
('Groceries', 1, 1),  
('Gas', 1, 1),  
('Medical Expenses', 1, 1),  
('Childcare', 1, 1),  
('Pet Care', 1, 1),  
('Dining Out', 0, 1),  
('Entertainment', 0, 1),  
('Clothing', 0, 1);

INSERT INTO Users (UserName, Email, password_hash, created_at, updated_at)
VALUES 
('JohnDough', 'johndough@example.com', 'hashedpassword1', GETDATE(), GETDATE()), 
('HaileyRichards', 'haileyrichards@example.com', 'hashedpassword2', GETDATE(), GETDATE());

INSERT INTO Bills (UserId, bill_name, Amount, due_date, CategoryId, Paid, created_at, updated_at)
VALUES
(1, 'Rent', 1200, '2024-11-01', 1, 0, GETDATE(), GETDATE()),  -- Unpaid Rent bill for JohnDough
(1, 'Utilities', 200, '2024-11-05', 2, 1, GETDATE(), GETDATE()), -- Paid utilities bill for JohnDough
(2, 'Groceries', 300, '2024-11-01', 8, 1, GETDATE(), GETDATE()), -- Paid groceries bill for HaileyRichards
(2, 'Rent', 1000, '2024-11-01', 1, 0, GETDATE(), GETDATE()); -- Unpaid rent bill for HaileyRichards

INSERT INTO Savings (UserId, savings_amount, created_at, updated_at)
VALUES
(1, 500, GETDATE(), GETDATE()), -- Monthly savings for JohnDough
(2, 700, GETDATE(), GETDATE()); -- Monthly savings for HaileyRichards

INSERT INTO Transactions (UserId, transaction_name, Amount, CategoryId, Date, created_at)
VALUES
(1, 'Groceries Shopping', 150, 8, '2024-10-15', GETDATE()), -- JohnDough transaction for groceries
(1, 'Dining Out', 50, 13, '2024-10-20', GETDATE()), -- JohnDough transaction for dining out
(2, 'Groceries Shopping', 120, 8, '2024-10-10', GETDATE()), -- HaileyRichards transaction for groceries
(2, 'Entertainment', 100, 14, '2024-10-12', GETDATE()); -- HaileyRichards transaction for entertainment
