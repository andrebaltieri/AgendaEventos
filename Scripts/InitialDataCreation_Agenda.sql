--CREATE INITIAL DATA FOR DATABASE AGENDA

USE [Agenda]
GO

--INSERTING CATEGORIES
INSERT INTO [Category](Title, [Description], Active) VALUES('Desenvolvimento Backend', 'Desenvolvimento Backend refere-se ao desenvolvimento "server side" (no lado do servidor) onde focamos principalmente em como o site funciona.', 1);
INSERT INTO [Category](Title, [Description], Active) VALUES('Desenvolvimento Frontend', 'O foco do Desenvolvimento Frontend e o que alguns chamam de desenvolvimento "client side" (no lado do cliente).', 1);
INSERT INTO [Category](Title, [Description], Active) VALUES('DevOps', 'DevOps e um conjunto de praticas que combinam Desenvolvimento de Software com Operacoes de TI.', 1);
INSERT INTO [Category](Title, [Description], Active) VALUES('Categoria Desativada de Teste', 'Categoria criada somente para ficar com status Active=0', 0);


GO


--INSERTING ROLES
INSERT INTO [Role](Title, [Description]) VALUES('Administrador', 'Usuario administrador do sistema de Agenda.');
INSERT INTO [Role](Title, [Description]) VALUES('Organizador', 'Cria e organiza eventos.');
INSERT INTO [Role](Title, [Description]) VALUES('Palestrante', 'Apresenta eventos.');
INSERT INTO [Role](Title, [Description]) VALUES('Participante', 'Se inscreve e participa de eventos.');
INSERT INTO [Role](Title, [Description]) VALUES('Role de teste', 'So pra testar a exclusao.');


GO


--INSERTING USERS
INSERT INTO [User]([Name], Email, [Password]) VALUES ('admin', 'admin@balta.io', 'admin');
INSERT INTO [User]([Name], Email, [Password]) VALUES ('organizador', 'organizador@balta.io', 'teste');
INSERT INTO [User]([Name], Email, [Password]) VALUES ('palestrante', 'palestrante@balta.io', 'teste_do_palestrante');
INSERT INTO [User]([Name], Email, [Password]) VALUES ('participante', 'participante@gmail.com', 'participei');
INSERT INTO [User]([Name], Email, [Password]) VALUES ('outroadmin', 'outroadmin@gmail.com', 'teste');


GO


--SETTING USER ADMIN AS AN ADMINISTRATOR
INSERT INTO [UserRoles](UserId, RoleId) 
VALUES(
	(SELECT ID FROM [USER] WHERE [NAME]='admin'), 
	(SELECT ID FROM [Role] where [TITLE] = 'Administrador'));

--SETTING USER ORGANIZADOR AS AN ORGANIZADOR
INSERT INTO [UserRoles](UserId, RoleId) 
VALUES(
	(SELECT ID FROM [USER] WHERE [NAME]='organizador'), 
	(SELECT ID FROM [Role] where [TITLE] = 'Organizador'));

--SETTING USER PALESTRANTE AS A PALESTRANTE
INSERT INTO [UserRoles](UserId, RoleId) 
VALUES(
	(SELECT ID FROM [USER] WHERE [NAME]='palestrante'), 
	(SELECT ID FROM [Role] where [TITLE] = 'Palestrante'));

--SETTING USER PARTICIPANTE AS A PARTICIPANTE
INSERT INTO [UserRoles](UserId, RoleId) 
VALUES(
	(SELECT ID FROM [USER] WHERE [NAME]='participante'), 
	(SELECT ID FROM [Role] where [TITLE] = 'Participante'));

--SETTING USER OUTROADMIN AS ADMIN, ORGANIZADOR E PARTICIPANTE
INSERT INTO [UserRoles](UserId, RoleId) 
VALUES(
	(SELECT ID FROM [USER] WHERE [NAME]='outroadmin'), 
	(SELECT ID FROM [Role] where [TITLE] = 'Administrador'));
INSERT INTO [UserRoles](UserId, RoleId) 
VALUES(
	(SELECT ID FROM [USER] WHERE [NAME]='outroadmin'), 
	(SELECT ID FROM [Role] where [TITLE] = 'Organizador'));
INSERT INTO [UserRoles](UserId, RoleId) 
VALUES(
	(SELECT ID FROM [USER] WHERE [NAME]='outroadmin'), 
	(SELECT ID FROM [Role] where [TITLE] = 'Participante'));


GO


SELECT * FROM [Category];
SELECT * FROM [Role];
SELECT * FROM [User];
SELECT * FROM [UserRoles];
SELECT * FROM [Event];


--DELETING ALL DATA, IF NECESSARY
/*
DELETE FROM [Category];
DELETE FROM [UserRoles];
DELETE FROM [Event];
DELETE FROM [Role];
DELETE FROM [User];
*/
