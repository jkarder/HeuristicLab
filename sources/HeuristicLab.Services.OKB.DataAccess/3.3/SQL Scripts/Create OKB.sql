USE [master]
GO

/****** Object:  Database [HeuristicLab.OKB]    Script Date: 07/01/2009 13:11:18 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'HeuristicLab.OKB')
DROP DATABASE [HeuristicLab.OKB]
GO

USE [master]
GO

/****** Object:  Database [HeuristicLab.OKB]    Script Date: 07/01/2009 13:11:18 ******/
CREATE DATABASE [HeuristicLab.OKB] ON  PRIMARY 
( NAME = N'HeuristicLab.OKB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.HeuristicLab.OKB\MSSQL\DATA\HeuristicLab.OKB.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB ), 
 FILEGROUP [DATA] 
( NAME = N'HeuristicLab.OKB_DATA', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.HeuristicLab.OKB\MSSQL\DATA\HeuristicLab.OKB_DATA.ndf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HeuristicLab.OKB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.HeuristicLab.OKB\MSSQL\DATA\HeuristicLab.OKB_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [HeuristicLab.OKB] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HeuristicLab.OKB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [HeuristicLab.OKB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET ARITHABORT OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [HeuristicLab.OKB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [HeuristicLab.OKB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [HeuristicLab.OKB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [HeuristicLab.OKB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [HeuristicLab.OKB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [HeuristicLab.OKB] SET  READ_WRITE 
GO

ALTER DATABASE [HeuristicLab.OKB] SET RECOVERY FULL 
GO

ALTER DATABASE [HeuristicLab.OKB] SET  MULTI_USER 
GO

ALTER DATABASE [HeuristicLab.OKB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [HeuristicLab.OKB] SET DB_CHAINING OFF 
GO


