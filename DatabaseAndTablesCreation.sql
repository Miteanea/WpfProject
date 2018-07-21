 CREATE DATABASE [Personal Shopper]
 GO

 create table [Categories]
 (
     [Id] int identity(1,1) not null,
     constraint PK_Categories primary key (Id),
     [NameCategory] nvarchar(50) not null,
 )

 create table [Expenses]
 (
     [Id] int identity(1,1) not null,
     constraint PK_Expenses primary key (Id),
     [Name] nvarchar(100) not null,
     [Quantity] decimal(18,2) not null, 
     [Price] decimal (18,2) not null, 
     [Date] datetime not null,
     [Category] nvarchar(50) not null 
     )

     insert into [Expenses]
     (Name, quantity, price, date, category)
     values 
     ('prod53', 9, 39.79, '2018/03/24 12:00:00', 'Games(5)'), 
 ('prod12', 2, 5.19, '2017/12/11 08:00:00', 'Clothes(1)'), 
 ('prod14', 1, 65.39, '2018/06/18 08:00:00', 'Clothes(1)'), 
 ('prod24', 5, 84.99, '2017/12/28 08:00:00', 'Books(2)'), 
 ('prod35', 1, 28.79, '2018/01/09 08:00:00', 'Food(3)'), 
 ('prod15', 7, 91.49, '2018/02/12 08:00:00', 'Clothes(1)'), 
 ('prod13', 4, 71.99, '2017/11/01 04:00:00', 'Clothes(1)'), 
 ('prod53', 5, 36.49, '2018/04/30 10:00:00', 'Games(5)'), 
 ('prod25', 7, 67.59, '2018/04/27 12:00:00', 'Books(2)'), 
 ('prod41', 1, 17.99, '2018/04/16 10:00:00', 'Electronics(4)'), 
 ('prod24', 8, 92.99, '2017/08/12 10:00:00', 'Books(2)'), 
 ('prod55', 8, 11.39, '2018/03/17 10:00:00', 'Games(5)'), 
 ('prod13', 7, 60.89, '2017/07/14 02:00:00', 'Clothes(1)'), 
 ('prod01', 2, 17.99, '2018/07/15 02:00:00', 'Alcohol(0)'), 
 ('prod35', 9, 46.29, '2018/04/15 12:00:00', 'Food(3)'), 
 ('prod43', 9, 94.99, '2017/08/05 04:00:00', 'Electronics(4)'), 
 ('prod44', 1, 55.59, '2017/09/07 04:00:00', 'Electronics(4)'), 
 ('prod05', 9, 20.79, '2018/05/20 02:00:00', 'Alcohol(0)'), 
 ('prod52', 1, 46.59, '2017/09/15 08:00:00', 'Games(5)'), 
 ('prod02', 9, 80.89, '2018/06/24 08:00:00', 'Alcohol(0)')