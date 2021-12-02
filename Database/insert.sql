insert into OrderStates values('In elaborazione');
insert into OrderStates values('Confermato');
insert into OrderStates values('Cancellato');

insert into Products values('Scarpe in pelle', 10.99, 40, 1);
insert into Products values('Guanti tecnici', 5.49, 30, 1);
insert into Products values('Maglia nera', 29.99, 10, 1);
insert into Products values('Scarpe da ginnastica', 20, 10, 1);
insert into Products values('Camicia a quadri', 49.99, 25, 1);
insert into Products values('Camicia bianca', 75, 15, 1);
insert into Products values('Stivali in cuoio', 45.99, 20, 1);
insert into Products values('Sciarpa', 12, 5, 1);
insert into Products values('Cappello di lana', 8.99, 15, 1);
insert into Products values('Cappello con visiera', 19.99, 25, 1);
insert into Products values('Occhiali da sole', 64.99, 35, 1);
insert into Products values('Gonna in jeans', 39.99, 20, 1);
insert into Products values('Canotta da basket', 52, 30, 1);
insert into Products values('Leggins neri', 20, 15, 1);
insert into Products values('Jeans donna', 55, 10, 1);
insert into Products values('Jeans uomo', 59.99, 40, 1);
insert into Products values('Shorts colorati', 35, 25, 1);
insert into Products values('Calzini neri', 4.99, 35, 1);

insert into UserInfos values('Mario', 'Rossi', 'mario@rossi.com', 'IT', 1, 1, null, '1111111111');
insert into UserInfos values('Giulia', 'Verdi', 'giulia@verdi.com', 'IT', 0, 0, null, null);
insert into UserInfos values('Michele', 'Sala', 'michele@sala.com', 'ES', 0, 1, null, '1111111111');
insert into UserInfos values('Andrea', 'Tosto', 'andrea@tosto.com', 'IT', 1, 0, null, null);
insert into UserInfos values('Silvia', 'Sera', 'silvia@sera.com', 'FR', 1, 0, null, null);
insert into UserInfos values('Sara', 'Nero', 'sara@nero.com', 'IT', 1, 0, null, '1111111111');
insert into UserInfos values('Elisabetta', 'Sali', 'elisabetta@sali.com', 'FR', 1, 1, null, null);

insert into Orders values (null, null, 1, 0, 0, null);
insert into Orders values (null, null, 1, 0, 0, null);
insert into Orders values (5, null, 2, 0, 0, null);
insert into Orders values (null, null, 1, 0, 0, null);
insert into Orders values (null, null, 1, 0, 0, null);
insert into Orders values (null, null, 1, 0, 0, null);
insert into Orders values (6, null, 2, 0, 0, null);
insert into Orders values (2, null, 2, 0, 0, null);
insert into Orders values (null, null, 1, 0, 0, null);
insert into Orders values (null, null, 1, 0, 0, null);

insert into Orders_Products values(1, 12, 1);
insert into Orders_Products values(1, 8, 2);
insert into Orders_Products values(1, 5, 1);

insert into Orders_Products values(2, 12, 1);
insert into Orders_Products values(2, 8, 2);
insert into Orders_Products values(2, 5, 1);
insert into Orders_Products values(2, 3, 2);

insert into Orders_Products values(3, 12, 1);
insert into Orders_Products values(3, 8, 2);
insert into Orders_Products values(3, 5, 1);
insert into Orders_Products values(3, 3, 2);

insert into Orders_Products values(4, 10, 3);
insert into Orders_Products values(4, 16, 2);

insert into Orders_Products values(5, 7, 1);
insert into Orders_Products values(5, 16, 1);
insert into Orders_Products values(5, 17, 3);

insert into Orders_Products values(6, 4, 4);
insert into Orders_Products values(6, 11, 2);
insert into Orders_Products values(6, 18, 5);
insert into Orders_Products values(6, 14, 2);
insert into Orders_Products values(6, 7, 1);

insert into Orders_Products values(7, 2, 3);
insert into Orders_Products values(7, 5, 1);

insert into Orders_Products values(8, 15, 2);

insert into Orders_Products values(9, 6, 1);
insert into Orders_Products values(9, 8, 4);
insert into Orders_Products values(9, 14, 2);

insert into Orders_Products values(10, 15, 2);
insert into Orders_Products values(10, 15, 2);
insert into Orders_Products values(10, 15, 2);
insert into Orders_Products values(10, 15, 2);

insert into Orders_Products values(11, 3, 2);
insert into Orders_Products values(11, 8, 4);

insert into Orders_Products values(12, 1, 1);
insert into Orders_Products values(12, 6, 1);
insert into Orders_Products values(12, 8, 4);
insert into Orders_Products values(12, 11, 3);
insert into Orders_Products values(12, 14, 2);

insert into Coupons values('1111111111', 1, 5, NULL);
insert into Coupons values('2222222222', 1, 5, NULL);
insert into Coupons values('3333333333', 1, 5, NULL);
insert into Coupons values('4444444444', 0, 5, GETDATE());
insert into Coupons values('5555555555', 1, 5, NULL);
insert into Coupons values('6666666666', 1, 5, NULL);
insert into Coupons values('7777777777', 0, 5, GETDATE());
insert into Coupons values('8888888888', 1, 5, NULL);
insert into Coupons values('9999999999', 1, 5, NULL);

-- aggriorna prezzo totale su Orders
update o
set TotalPrice = t.sumOrder, TotalPriceWithoutDiscount = t.sumOrder
from Orders o
join 
( 
  select sum(op.Quantity * p.Price) sumOrder, OrdersId
  from Orders_Products op, Products p
  where op.ProductsId = p.Id
  group by OrdersId
) t
on t.OrdersId = o.Id;