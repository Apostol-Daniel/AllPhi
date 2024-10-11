# AllPhi

## Requirements
Practical coding test
I want you to create a REST api that allows the management of CUSTOMERS and ORDERS.
A customer has at least the following data:
-	First name ✅
-	Last name ✅
-	Email address, must be unique in the system ✅
-	Zero or more orders ✅

### An order has at least the following data:

-	Description, max 100 chars and must support Chinese characters ✅

Notes: .IsUnicode() required in AppDbContext

-	Price ✅
-	Creation date ✅

### The API should have the following capabilities:

-	Retrieve list of all customers ✅
-	Search customers by (partial) email address ✅
-	Create a customer ✅
-	Update a customer ❌
-	Create an order for a customer ✅
-	Retrieve all orders for a given a customer ✅
-	Search orders in a certain time window (for a specific customer or multiple customers) ✅

Note: at least one customer id has to be given as parameter
-	Cancel an order ✅

Note: orders can be cancelled continuously, even if order is already cancelled
-	Total amount spent by a customer ✅
-	Total revenue for a specific month ✅

### The implementation should have at least the following:

-	Use dotnet 8 with C#
-	The API should be RESTful (think about url, request methods, response codes, maybe even caching)
-	Use Mediatr for logic - with CQRS
-	Use a validation library - FluentValidation
-	Use EntityFramework for database access
-	Use IOC/DI, built in with dotnet or any other IOC container of your choice (Autofac…) - Basic microsoft DI
-	NO security/authentication/authorization needed!!


Sql script to run into mssql:

```
INSERT INTO Customers (FirstName, LastName, Email)
VALUES
(N'John', N'Doe', N'john.doe@example.com'),
(N'Jane', N'Smith', N'jane.smith@example.com'),
(N'Bob', N'Johnson', N'bob.johnson@example.com'),
(N'Alice', N'Brown', N'alice.brown@example.com'),
(N'Charlie', N'Wilson', N'charlie.wilson@example.com'),
(N'Diana', N'Davis', N'diana.davis@example.com'),
(N'Edward', N'Miller', N'edward.miller@example.com'),
(N'Fiona', N'Garcia', N'fiona.garcia@example.com'),
(N'George', N'Martinez', N'george.martinez@example.com'),
(N'Helen', N'Anderson', N'helen.anderson@example.com');

WITH NumberedCustomers AS (
SELECT Id, ROW_NUMBER() OVER (ORDER BY Id) AS RowNum
FROM Customers
)
INSERT INTO Orders (CustomerId, Description, Price, CreationDate, IsCancelled)
SELECT
c.Id,
CASE
WHEN (n % 7) = 0 THEN N'Order with 中文 - ' + CAST(n AS NVARCHAR(10))
ELSE N'Regular order - ' + CAST(n AS NVARCHAR(10))
END,
ROUND(RAND() * 500 + 50, 2),
DATEADD(DAY, -n % 60, GETDATE()),
CASE
WHEN n <= (SELECT COUNT(*) FROM NumberedCustomers) * 5 THEN 0
ELSE 1
END
FROM NumberedCustomers c
CROSS JOIN (SELECT TOP 7 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n FROM sys.objects) numbers;
```
