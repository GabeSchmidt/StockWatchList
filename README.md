# StockWatchList

This stock watch list has an auto complete search that allows you to add a stock/company to your watch list. 
It saves your watch list to a table with your windows user.
The watch list is displayed in a grid.
You can view the history of each stock/company by clicking on the history button for the stock/company you would like to view.

create table Logging.dbo.StocksWatchlist (
	ID int not null identity(1,1) primary key,
	UserName varchar(256) not null,
	StockSymbol varchar(10) not null
)

create table Logging.dbo.Stocklist (
	ID int not null identity(1,1) primary key,
	StockSymbol varchar(10) not null,
	CompanyName varchar(256),
	LastUpdated date
)
