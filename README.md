# StockWatchList

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
