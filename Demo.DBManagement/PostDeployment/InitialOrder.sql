INSERT INTO T_Order 
VALUES
(1, GETDATE(), 'Order placed online', 1579.97),
(2, DATEADD(DAY, -1, GETDATE()), 'Order placed over the phone', 1179.97),
(3, DATEADD(DAY, -7, GETDATE()), 'Order from regular customer', 89.98),
(4, DATEADD(MONTH, -1, GETDATE()), 'Order with expedited shipping', 199.99),
(1, GETDATE(), 'Order placed online', 109.98)

