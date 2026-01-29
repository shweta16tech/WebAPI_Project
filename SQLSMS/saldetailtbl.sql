create table saledetails
(
	saledetailid int identity primary key,
	sid int not null,
	pid int not null,
	quantity int,
	rate money,
	price money,
	total money,
	constraint FK_saledetails_sales foreign key (sid)
	references sales(sid)
	on delete cascade,
	constraint FK_saledetails_product foreign key (pid)
	references product(pid)
);