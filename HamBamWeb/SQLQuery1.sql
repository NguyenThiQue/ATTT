/*CREATE PROCEDURE InsertAccount (@username NCHAR(50), @pw NCHAR(255))
AS 
BEGIN
     INSERT INTO Login
     VALUES (@username, @pw)
END;
exec InsertAccount "que", "123"*/

/*CREATE PROCEDURE SelectAccountByUsername (@username NCHAR(50))
AS 
BEGIN
     SELECT * 
     FROM Login
     WHERE username=@username
END;
exec SelectAccountByUsername "que"*/
/*CREATE PROCEDURE SelectAccount (@username NCHAR(50), @pw NCHAR(255))
AS 
BEGIN
     SELECT * 
     FROM Login
     WHERE username=@username
	 AND password = @pw
END;
exec SelectAccount "que", "123"*/
