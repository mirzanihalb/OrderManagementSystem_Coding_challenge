create database OrderManagementSystem

create table Users(
	user_id int identity primary key,
	username varchar(50) unique not null,
	password varchar(255) not null,
	role varchar(25) not null)

create table Products(
	product_id int identity primary key,
	product_name varchar(50) not null,
	description varchar(255),
	price decimal(10,2) not null,
	quantity_in_stock int default 0,
	type VARCHAR(50) CHECK (type IN ('Electronics', 'Clothing')),
	)

create table Cart(
	cart_id int identity primary key,
	user_id int,
	product_id int,
	quantity int,
	foreign key (user_id) references Users(user_id) on delete cascade,
	foreign key (product_id) references Products(product_id) on delete cascade)

create table Orders(
	order_id int identity primary key,
	user_id int,
	order_date date not null,
	total_price decimal(10,2) not null,
	shipping_address varchar(255) not null,
	foreign key (user_id) references Users(user_id) on delete set null) 

create table Order_items(
	order_item_id int identity primary key,
	order_id int,
	product_id int,
	quantity int,
	foreign key (order_id) references Orders(order_id) on delete cascade,
	foreign key (product_id) references products(product_id) on delete cascade)



select * from users
select * from products
select * from products
select * from orders where order_id=1 and user_id =2
insert into users(username,password,role) values('taj','password','Admin');

select * from order_items