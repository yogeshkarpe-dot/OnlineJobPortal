create database JobPortal
use JobPortal

create table Contact(
ContactId int primary key identity(1,1) not null,
Name varchar(50),
Email varchar(50),
Subject varchar(100),
Message varchar(Max)
)

Create table Country(
CountryId int primary key identity(1,1),
CountryName varchar(50)
)

insert Country values ('India'), ('United States'), ('England'), ('China'), ('Japan'),('Germany'), ('Pakistan'),
						('South Africa'), ('South Korea'), ('Brazil'),('Canada'), ('Malaysia'), ('Australia'),
						('Italy'), ('Poland'), ('UAE'), ('Egypt'),('Bangladesh'), ('Philipines')

create table [User](
UserId int primary key identity(1,1),
UserName varchar(50),
Password varchar(50),
Name varchar(50),
Email varchar(50),
Mobile varchar(50),
TenthGrade varchar(50),
TwelthGrade varchar(50),
GraduationGrade varchar(50),
PostGraduationGrade varchar(50),
PHD varchar(50),
WorksOn varchar(50),
Experience varchar(50),
Resume varchar(max),
Address varchar(max),
Country varchar(50)
)

alter table [User]
add unique (UserName)

create table Jobs(
JobId int primary key identity(1,1),
Title varchar(50),
NoOfPost int,
Description varchar(max),
Qualification varchar(50),
Experience varchar(50),
Specialisation varchar(max),
LastDataToApply date,
Salary varchar(50),
JobType varchar(50),
CompanyName varchar(200),
CompanyImage varchar(500),
Website varchar(100),
Email varchar(50),
Address varchar(max),
Country varchar(50),
State varchar(50),
CreateDate datetime,
)

create table AppliedJobs(
AppliedJobId int primary key identity(1,1),
JobId int,
UserId int
)

