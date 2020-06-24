CREATE DATABASE [dbLoan]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dbLoan', FILENAME = N':)Database_Name(:' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'dbLoan_log', FILENAME = N':)Database_Log(:' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
ƒ
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dbLoan].[dbo].[sp_fulltext_database] @action = 'enable'
end
ƒ
ALTER DATABASE [dbLoan] SET ANSI_NULL_DEFAULT OFF 
ƒ
ALTER DATABASE [dbLoan] SET ANSI_NULLS OFF 
ƒ
ALTER DATABASE [dbLoan] SET ANSI_PADDING OFF 
ƒ
ALTER DATABASE [dbLoan] SET ANSI_WARNINGS OFF 
ƒ
ALTER DATABASE [dbLoan] SET ARITHABORT OFF 
ƒ
ALTER DATABASE [dbLoan] SET AUTO_CLOSE OFF 
ƒ
ALTER DATABASE [dbLoan] SET AUTO_SHRINK OFF 
ƒ
ALTER DATABASE [dbLoan] SET AUTO_UPDATE_STATISTICS ON 
ƒ
ALTER DATABASE [dbLoan] SET CURSOR_CLOSE_ON_COMMIT OFF 
ƒ
ALTER DATABASE [dbLoan] SET CURSOR_DEFAULT  GLOBAL 
ƒ
ALTER DATABASE [dbLoan] SET CONCAT_NULL_YIELDS_NULL OFF 
ƒ
ALTER DATABASE [dbLoan] SET NUMERIC_ROUNDABORT OFF 
ƒ
ALTER DATABASE [dbLoan] SET QUOTED_IDENTIFIER OFF 
ƒ
ALTER DATABASE [dbLoan] SET RECURSIVE_TRIGGERS OFF 
ƒ
ALTER DATABASE [dbLoan] SET  DISABLE_BROKER 
ƒ
ALTER DATABASE [dbLoan] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
ƒ
ALTER DATABASE [dbLoan] SET DATE_CORRELATION_OPTIMIZATION OFF 
ƒ
ALTER DATABASE [dbLoan] SET TRUSTWORTHY OFF 
ƒ
ALTER DATABASE [dbLoan] SET ALLOW_SNAPSHOT_ISOLATION OFF 
ƒ
ALTER DATABASE [dbLoan] SET PARAMETERIZATION SIMPLE 
ƒ
ALTER DATABASE [dbLoan] SET READ_COMMITTED_SNAPSHOT OFF 
ƒ
ALTER DATABASE [dbLoan] SET HONOR_BROKER_PRIORITY OFF 
ƒ
ALTER DATABASE [dbLoan] SET RECOVERY SIMPLE 
ƒ
ALTER DATABASE [dbLoan] SET  MULTI_USER 
ƒ
ALTER DATABASE [dbLoan] SET PAGE_VERIFY CHECKSUM  
ƒ
ALTER DATABASE [dbLoan] SET DB_CHAINING OFF 
ƒ
ALTER DATABASE [dbLoan] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
ƒ
ALTER DATABASE [dbLoan] SET TARGET_RECOVERY_TIME = 60 SECONDS 
ƒ
ALTER DATABASE [dbLoan] SET DELAYED_DURABILITY = DISABLED 
ƒ
USE [dbLoan]
ƒ
/****** Object:  Table [dbo].[tblAccount]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblAccount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Personnel_Id] [int] NULL,
	[Account_PaymentType_Id] [tinyint] NULL,
	[Account_TransactionType_Id] [tinyint] NULL,
	[AccountAmount] [bigint] NULL,
	[AccountReceiptNumber] [nvarchar](15) NULL,
	[AccountCurrentBalance] [bigint] NULL,
	[AccountPaymentDate] [nvarchar](10) NULL,
	[AccountDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblAccountType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblAccountType](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[AccountType] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblBankAccount]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblBankAccount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BankAccountBankName] [nvarchar](50) NULL,
	[BankAccountBranchName] [nvarchar](50) NULL,
	[BankAccountNum] [nvarchar](20) NULL,
	[BankAccountCardNum] [nvarchar](20) NULL,
	[BankAccountInitialBalance] [bigint] NULL,
	[BankAccountDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblBankAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblBlockType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblBlockType](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[BlockType] [nvarchar](20) NULL,
 CONSTRAINT [PK_tblBlockType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblChargeMonthly]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblChargeMonthly](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChargeMonthly_Personnel_Id] [int] NULL,
	[ChargeMonthlyStartDate] [nvarchar](10) NULL,
	[ChargeMonthlyEndDate] [nvarchar](10) NULL,
	[ChargeMonthlyCharge] [bigint] NULL,
	[ChargeMonthlyDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblChargeMonthly] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblChMoPay]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblChMoPay](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChMoPay_ChargeMonthly_Id] [int] NULL,
	[ChMoPay_Account_Id] [int] NULL,
	[ChMoPayDueAmount] [bigint] NULL,
	[ChMoPayDueDate] [nvarchar](10) NULL,
	[ChMoPayDelayMonth] [smallint] NULL,
 CONSTRAINT [PK_tblChMoPay] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblFeeIncome]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblFeeIncome](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FeeIncome_FeeIncomeType_Id] [tinyint] NULL,
	[FeeIncome_PaymentType_Id] [tinyint] NULL,
	[FeeIncomeDate] [nvarchar](10) NULL,
	[FeeIncomeAmount] [bigint] NULL,
	[FeeIncomeReceiptNumber] [nvarchar](15) NULL,
	[FeeIncomeDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_FeeIncome] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblFeeIncomeType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblFeeIncomeType](
	[id] [tinyint] IDENTITY(1,1) NOT NULL,
	[FeeIncomeType] [nvarchar](10) NULL,
 CONSTRAINT [PK_tblFeeIncomeType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblGuaranteeType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblGuaranteeType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GuaranteeType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblGuaranteeType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblGuarantor]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblGuarantor](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Guarantor_Loan_Id] [int] NULL,
	[Guarantor_Info_Id] [int] NULL,
	[Guarantor_GuaranteeType_Id] [tinyint] NULL,
	[Guarantor_BlockType_Id] [tinyint] NULL,
	[GuarantorReceiptNumber] [nvarchar](15) NULL,
	[GuarantorAmount] [bigint] NULL,
	[GuarantorBlockAmount] [bigint] NULL,
	[GuarantorBlock] [bit] NULL,
	[GuarantorDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblGuaInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InfoFirstName] [nvarchar](50) NULL,
	[InfoLastName] [nvarchar](50) NULL,
	[InfoFatherName] [nvarchar](50) NULL,
	[InfoNationalCode] [nvarchar](10) NULL,
	[InfoCode] [nvarchar](10) NULL,
	[InfoGender] [nvarchar](1) NULL,
	[InfoBirthDay] [nvarchar](10) NULL,
	[InfoBirthPlace] [nvarchar](50) NULL,
	[InfoMarried] [nvarchar](1) NULL,
	[InfoTell] [nvarchar](11) NULL,
	[InfoMobile] [nvarchar](11) NULL,
	[InfoEmail] [nvarchar](200) NULL,
	[InfoPostalCode] [nvarchar](10) NULL,
	[InfoAddress] [nvarchar](max) NULL,
	[InfoJobName] [nvarchar](50) NULL,
	[InfoJobPlaceName] [nvarchar](50) NULL,
	[InfoJobTell] [nvarchar](11) NULL,
	[InfoJobFax] [nvarchar](11) NULL,
	[InfoJobAddress] [nvarchar](max) NULL,
	[InfoImage] [nvarchar](max) NULL,
	[InfoDescription] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblInstallment]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblInstallment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Installment_Loan_Id] [int] NULL,
	[Installment_PaymentType_Id] [tinyint] NULL,
	[InstallmentDueAmount] [bigint] NULL,
	[InstallmentAmount] [bigint] NULL,
	[InstallmentReceiptNumber] [nvarchar](15) NULL,
	[InstallmentTotalPaid] [bigint] NULL,
	[InstallmentRemaining] [bigint] NULL,
	[InstallmentPaymentDate] [nvarchar](10) NULL,
	[InstallmentDueDate] [nvarchar](10) NULL,
	[InstallmentDelayDay] [smallint] NULL,
	[InstallmentDescription] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblInstitution]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblInstitution](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Institution] [nvarchar](200) NULL,
 CONSTRAINT [PK_tblInstitution] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblIntroducer]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblIntroducer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Introducer_Loan_Id] [int] NULL,
	[Introducer_IntroducerType_Id] [tinyint] NULL,
	[Introducer_Info_Id] [int] NULL,
	[Introducer_Institution_Id] [smallint] NULL,
	[IntroducerDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblIntroducer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblIntroducerType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblIntroducerType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[IntroducerType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblIntroducerType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblLicense]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblLicense](
	[Id] [int] NOT NULL,
	[AppLicense] [nvarchar](40) NULL,
	[AppVersion] [nvarchar](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblLoan]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblLoan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Loan_Personnel_Id] [int] NULL,
	[Loan_LoanType_Id] [tinyint] NULL,
	[LoanAmount] [bigint] NULL,
	[LoanDate] [nvarchar](10) NULL,
	[LoanInstallmentNum] [tinyint] NULL,
	[LoanInstallmentFirstPayDate] [nvarchar](10) NULL,
	[LoanInstallmentInterspace] [tinyint] NULL,
	[LoanPayOff] [bit] NULL,
	[LoanDescription] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblLoanFund]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblLoanFund](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[LoanFundName] [nvarchar](100) NULL,
	[LoanFundTell1] [nvarchar](11) NULL,
	[LoanFundTell2] [nvarchar](11) NULL,
	[LoanFundFax] [nvarchar](11) NULL,
	[LoanFundEmail] [nvarchar](200) NULL,
	[LoanFundPostalCode] [nvarchar](10) NULL,
	[LoanFundAddress] [nvarchar](max) NULL,
	[LoanFundInitialBalance] [bigint] NULL,
	[LoanFundPenalty] [bigint] NULL,
	[LoanFundWagePercent] [nvarchar](3) NULL,
	[LoanFundLogo] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblLoanFund] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblLoanType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblLoanType](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[LoanType] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblPaymentType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblPaymentType](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[PaymentType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblPaymentType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblPerAccType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblPerAccType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PerAccType_Personnel_Id] [int] NULL,
	[PerAccType_AccountType_Id] [int] NULL,
	[PerAccTypeAccountNumber] [nvarchar](15) NULL,
 CONSTRAINT [PK_tblPerAccType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblPersonnel]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblPersonnel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Personnel_Info_Id] [int] NULL,
	[PersonnelId] [nvarchar](10) NULL,
	[PersonnelMembership] [nvarchar](1) NULL,
	[PersonnelMembershipDate] [nvarchar](10) NULL,
	[PersonnelBARCode] [nvarchar](max) NULL,
	[PersonnelQRCode] [nvarchar](max) NULL,
	[PersonnelSignature] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblPersonnel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblPostType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblPostType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostType] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblSecurityAccess]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblSecurityAccess](
	[Id] [int] NOT NULL,
	[Time] [nvarchar](19) NULL,
	[Counter] [nvarchar](1) NULL,
 CONSTRAINT [PK_tblSecurityAccess] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblSecurityQuestion]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblSecurityQuestion](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[SecurityQuestion] [nvarchar](200) NULL,
 CONSTRAINT [PK_tblSecurityQuestion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblSundry]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblSundry](
	[Id] [int] NOT NULL,
	[RegisteredAdminPassword] [bit] NULL,
 CONSTRAINT [PK_tblSundry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblTransactionType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblTransactionType](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[TransactionType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblTransactionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblUser]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User_PostType_Id] [tinyint] NULL,
	[User_SecurityQuestion_Id] [tinyint] NULL,
	[UserFirstName] [nvarchar](50) NULL,
	[UserLastName] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NULL,
	[UserPassword] [nvarchar](60) NULL,
	[UserMobileNumber] [nvarchar](11) NULL,
	[UserEmail] [nvarchar](200) NULL,
	[UserAnswer] [nvarchar](100) NULL,
	[UserRegistrationDate] [nvarchar](19) NULL,
	[UserImage] [nvarchar](max) NULL,
	[UserDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblWage]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblWage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Wage_Loan_Id] [int] NULL,
	[Wage_WageType_Id] [tinyint] NULL,
	[Wage_WageCalculationType_Id] [tinyint] NULL,
	[WagePercent] [nvarchar](4) NULL,
	[WageAmount] [bigint] NULL,
	[WageDescription] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblWageCalculationType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblWageCalculationType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WageCalculationType] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  Table [dbo].[tblWageType]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE TABLE [dbo].[tblWageType](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[WageType] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ƒ
/****** Object:  View [dbo].[viewAccountInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE VIEW [dbo].[viewAccountInfo] AS SELECT        dbo.tblAccount.Id, dbo.tblAccount.Account_TransactionType_Id, dbo.tblAccount.AccountAmount, dbo.tblAccount.AccountReceiptNumber, dbo.tblAccount.AccountCurrentBalance, dbo.tblAccount.AccountPaymentDate,                           dbo.tblAccount.AccountDescription, dbo.tblTransactionType.TransactionType, dbo.tblPaymentType.PaymentType, dbo.tblAccount.Account_PaymentType_Id, dbo.tblAccount.Account_Personnel_Id FROM            dbo.tblAccount INNER JOIN                          dbo.tblTransactionType ON dbo.tblTransactionType.Id = dbo.tblAccount.Account_TransactionType_Id INNER JOIN                          dbo.tblPaymentType ON dbo.tblPaymentType.Id = dbo.tblAccount.Account_PaymentType_Id   
ƒ
/****** Object:  View [dbo].[viewChMoPayInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE VIEW [dbo].[viewChMoPayInfo] AS SELECT        dbo.tblAccount.Id, dbo.tblAccount.Account_Personnel_Id, dbo.tblAccount.Account_PaymentType_Id, dbo.tblAccount.Account_TransactionType_Id, dbo.tblAccount.AccountAmount,                           dbo.tblAccount.AccountReceiptNumber, dbo.tblAccount.AccountCurrentBalance, dbo.tblAccount.AccountPaymentDate, dbo.tblAccount.AccountDescription, dbo.tblChMoPay.Id AS Expr1,                           dbo.tblChMoPay.ChMoPay_ChargeMonthly_Id, dbo.tblChMoPay.ChMoPayDueAmount, dbo.tblChMoPay.ChMoPayDueDate, dbo.tblChMoPay.ChMoPayDelayMonth, dbo.tblTransactionType.TransactionType,                           dbo.tblPaymentType.PaymentType FROM            dbo.tblAccount INNER JOIN                          dbo.tblChMoPay ON dbo.tblChMoPay.ChMoPay_Account_Id = dbo.tblAccount.Id INNER JOIN                          dbo.tblTransactionType ON dbo.tblTransactionType.Id = dbo.tblAccount.Account_TransactionType_Id INNER JOIN                          dbo.tblPaymentType ON dbo.tblPaymentType.Id = dbo.tblAccount.Account_PaymentType_Id   
ƒ
/****** Object:  View [dbo].[viewFeeIncomeInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE VIEW [dbo].[viewFeeIncomeInfo] AS SELECT        dbo.tblFeeIncome.*, dbo.tblFeeIncomeType.FeeIncomeType FROM            dbo.tblFeeIncome INNER JOIN                          dbo.tblFeeIncomeType ON dbo.tblFeeIncome.FeeIncome_FeeIncomeType_Id = dbo.tblFeeIncomeType.id   
ƒ
/****** Object:  View [dbo].[viewInsLoan]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE VIEW [dbo].[viewInsLoan] AS SELECT        dbo.tblLoan.Loan_Personnel_Id, dbo.tblLoan.LoanPayOff, dbo.tblInstallment.InstallmentDueAmount, dbo.tblInstallment.InstallmentAmount, dbo.tblGuarantor.Guarantor_Info_Id,                           dbo.tblGuarantor.Guarantor_BlockType_Id, dbo.tblGuarantor.GuarantorBlock, dbo.tblGuarantor.GuarantorBlockAmount, dbo.tblLoan.Id FROM            dbo.tblLoan INNER JOIN                          dbo.tblInstallment ON dbo.tblLoan.Id = dbo.tblInstallment.Installment_Loan_Id INNER JOIN                          dbo.tblGuarantor ON dbo.tblLoan.Id = dbo.tblGuarantor.Guarantor_Loan_Id   
ƒ
/****** Object:  View [dbo].[viewLoanInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE VIEW [dbo].[viewLoanInfo] AS SELECT        dbo.tblLoan.Id, dbo.tblLoan.Loan_Personnel_Id, dbo.tblLoan.Loan_LoanType_Id, dbo.tblLoan.LoanAmount, dbo.tblLoan.LoanDate, dbo.tblLoan.LoanInstallmentNum, dbo.tblLoan.LoanInstallmentFirstPayDate,                           dbo.tblLoan.LoanInstallmentInterspace, dbo.tblLoan.LoanPayOff, dbo.tblLoan.LoanDescription, dbo.tblLoanType.LoanType, dbo.tblWage.Wage_WageType_Id, dbo.tblWage.Wage_WageCalculationType_Id,                           dbo.tblWage.WagePercent, dbo.tblWage.WageAmount, dbo.tblWage.WageDescription, dbo.tblWageType.WageType FROM            dbo.tblLoan INNER JOIN                          dbo.tblLoanType ON dbo.tblLoanType.Id = dbo.tblLoan.Loan_LoanType_Id INNER JOIN                          dbo.tblWage ON dbo.tblWage.Wage_Loan_Id = dbo.tblLoan.Id INNER JOIN                          dbo.tblWageType ON dbo.tblWageType.Id = dbo.tblWage.Wage_WageType_Id   
ƒ
/****** Object:  View [dbo].[viewPersonnelInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE VIEW [dbo].[viewPersonnelInfo] AS SELECT        dbo.tblPersonnel.PersonnelId, dbo.tblPersonnel.PersonnelMembership, dbo.tblPersonnel.PersonnelMembershipDate, dbo.tblPersonnel.PersonnelBARCode, dbo.tblPersonnel.PersonnelQRCode,                           dbo.tblInfo.InfoNationalCode, dbo.tblInfo.InfoFirstName, dbo.tblInfo.InfoLastName, dbo.tblInfo.InfoFatherName, dbo.tblInfo.InfoCode, dbo.tblInfo.InfoGender, dbo.tblInfo.InfoBirthDay, dbo.tblInfo.InfoBirthPlace,                           dbo.tblInfo.InfoMarried, dbo.tblInfo.InfoTell, dbo.tblInfo.InfoMobile, dbo.tblInfo.InfoEmail, dbo.tblInfo.InfoPostalCode, dbo.tblInfo.InfoAddress, dbo.tblInfo.InfoJobName, dbo.tblInfo.InfoJobPlaceName,                           dbo.tblInfo.InfoJobTell, dbo.tblInfo.InfoJobFax, dbo.tblInfo.InfoJobAddress, dbo.tblInfo.InfoImage, dbo.tblInfo.InfoDescription, dbo.tblPersonnel.PersonnelSignature, dbo.tblPersonnel.Id,                           dbo.tblPersonnel.Personnel_Info_Id FROM            dbo.tblPersonnel INNER JOIN                          dbo.tblInfo ON dbo.tblInfo.Id = dbo.tblPersonnel.Personnel_Info_Id   
ƒ
SET IDENTITY_INSERT [dbo].[tblAccountType] ON 

ƒ
INSERT [dbo].[tblAccountType] ([Id], [AccountType]) VALUES (1, N'قرض الحسنه')
ƒ
SET IDENTITY_INSERT [dbo].[tblAccountType] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblBlockType] ON 

ƒ
INSERT [dbo].[tblBlockType] ([Id], [BlockType]) VALUES (1, N'عدم مسدود سازی')
ƒ
INSERT [dbo].[tblBlockType] ([Id], [BlockType]) VALUES (2, N'تا تسویه وام')
ƒ
INSERT [dbo].[tblBlockType] ([Id], [BlockType]) VALUES (3, N'تا مبلغ تعیین شده')
ƒ
SET IDENTITY_INSERT [dbo].[tblBlockType] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblFeeIncomeType] ON 

ƒ
INSERT [dbo].[tblFeeIncomeType] ([id], [FeeIncomeType]) VALUES (1, N'درآمد')
ƒ
INSERT [dbo].[tblFeeIncomeType] ([id], [FeeIncomeType]) VALUES (2, N'هزینه')
ƒ
SET IDENTITY_INSERT [dbo].[tblFeeIncomeType] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblGuaranteeType] ON 

ƒ
INSERT [dbo].[tblGuaranteeType] ([Id], [GuaranteeType]) VALUES (1, N'بدون ضمانت')
ƒ
INSERT [dbo].[tblGuaranteeType] ([Id], [GuaranteeType]) VALUES (2, N'چک')
ƒ
INSERT [dbo].[tblGuaranteeType] ([Id], [GuaranteeType]) VALUES (3, N'سفته')
ƒ
INSERT [dbo].[tblGuaranteeType] ([Id], [GuaranteeType]) VALUES (4, N'سند منقول')
ƒ
INSERT [dbo].[tblGuaranteeType] ([Id], [GuaranteeType]) VALUES (5, N'سند غیر منقول')
ƒ
INSERT [dbo].[tblGuaranteeType] ([Id], [GuaranteeType]) VALUES (6, N'وجه نقد')
ƒ
INSERT [dbo].[tblGuaranteeType] ([Id], [GuaranteeType]) VALUES (7, N'طلا و مسکوکات')
ƒ
INSERT [dbo].[tblGuaranteeType] ([Id], [GuaranteeType]) VALUES (8, N'سایر')
ƒ
SET IDENTITY_INSERT [dbo].[tblGuaranteeType] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblInstitution] ON 

ƒ
INSERT [dbo].[tblInstitution] ([id], [Institution]) VALUES (1, N'کمیته امداد امام خمینی')
ƒ
INSERT [dbo].[tblInstitution] ([id], [Institution]) VALUES (2, N'بنیاد مستضعفان انقلاب اسلامی')
ƒ
INSERT [dbo].[tblInstitution] ([id], [Institution]) VALUES (3, N'سازمان بهزیستی ایران')
ƒ
INSERT [dbo].[tblInstitution] ([id], [Institution]) VALUES (4, N'سازمان اوقاف و امور خیریه')
ƒ
INSERT [dbo].[tblInstitution] ([id], [Institution]) VALUES (5, N'آموزش و پرورش')
ƒ
INSERT [dbo].[tblInstitution] ([id], [Institution]) VALUES (6, N'سازمان تأمین اجتماعی')
ƒ
INSERT [dbo].[tblInstitution] ([id], [Institution]) VALUES (7, N'سازمان تبلیغات اسلامی')
ƒ
INSERT [dbo].[tblInstitution] ([id], [Institution]) VALUES (8, N'جهاد سازندگی')
ƒ
INSERT [dbo].[tblInstitution] ([id], [Institution]) VALUES (9, N'بنیاد شهید و امور ایثارگران')
ƒ
SET IDENTITY_INSERT [dbo].[tblInstitution] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblIntroducerType] ON 

ƒ
INSERT [dbo].[tblIntroducerType] ([id], [IntroducerType]) VALUES (1, N'حقیقی')
ƒ
INSERT [dbo].[tblIntroducerType] ([id], [IntroducerType]) VALUES (2, N'حقوقی')
ƒ
SET IDENTITY_INSERT [dbo].[tblIntroducerType] OFF
ƒ
INSERT [dbo].[tblLicense] ([Id], [AppLicense], [AppVersion]) VALUES (1, NULL, NULL)
ƒ
SET IDENTITY_INSERT [dbo].[tblLoanType] ON 

ƒ
INSERT [dbo].[tblLoanType] ([Id], [LoanType]) VALUES (1, N'اقساطی')
ƒ
INSERT [dbo].[tblLoanType] ([Id], [LoanType]) VALUES (2, N'مضاربه ای')
ƒ
INSERT [dbo].[tblLoanType] ([Id], [LoanType]) VALUES (3, N'ضروری')
ƒ
SET IDENTITY_INSERT [dbo].[tblLoanType] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblPaymentType] ON 

ƒ
INSERT [dbo].[tblPaymentType] ([Id], [PaymentType]) VALUES (1, N'نقدی')
ƒ
INSERT [dbo].[tblPaymentType] ([Id], [PaymentType]) VALUES (2, N'چک')
ƒ
INSERT [dbo].[tblPaymentType] ([Id], [PaymentType]) VALUES (3, N'فیش بانکی')
ƒ
INSERT [dbo].[tblPaymentType] ([Id], [PaymentType]) VALUES (4, N'کارت خوان')
ƒ
SET IDENTITY_INSERT [dbo].[tblPaymentType] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblPostType] ON 

ƒ
INSERT [dbo].[tblPostType] ([Id], [PostType]) VALUES (1, N'مدیریت')
ƒ
SET IDENTITY_INSERT [dbo].[tblPostType] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblSecurityQuestion] ON 

ƒ
INSERT [dbo].[tblSecurityQuestion] ([Id], [SecurityQuestion]) VALUES (1, N'نام اولین معلم شما چیست؟')
ƒ
INSERT [dbo].[tblSecurityQuestion] ([Id], [SecurityQuestion]) VALUES (2, N'نام گل مورد علاقه شما چیست؟')
ƒ
INSERT [dbo].[tblSecurityQuestion] ([Id], [SecurityQuestion]) VALUES (3, N'رنگ مورد علاقه شما چیست؟')
ƒ
INSERT [dbo].[tblSecurityQuestion] ([Id], [SecurityQuestion]) VALUES (4, N'نام فیلم مورد علاقه شما چیست؟')
ƒ
INSERT [dbo].[tblSecurityQuestion] ([Id], [SecurityQuestion]) VALUES (5, N'مکان مورد علاقه شما کجاست؟')
ƒ
SET IDENTITY_INSERT [dbo].[tblSecurityQuestion] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblTransactionType] ON 

ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (1, N'افتتاح حساب')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (2, N'واریز')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (3, N'واریز شارژ ماهانه')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (4, N'واریز سود')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (5, N'واریز وام')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (6, N'برداشت')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (7, N'مسدودی')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (8, N'انتقال وجه')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (9, N'مانده از شارژ ماهانه')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (10, N'مانده از قسط وام')
ƒ
INSERT [dbo].[tblTransactionType] ([Id], [TransactionType]) VALUES (11, N'حق عضویت')
ƒ
SET IDENTITY_INSERT [dbo].[tblTransactionType] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblWageCalculationType] ON 

ƒ
INSERT [dbo].[tblWageCalculationType] ([Id], [WageCalculationType]) VALUES (1, N'تعیین مبلغ')
ƒ
INSERT [dbo].[tblWageCalculationType] ([Id], [WageCalculationType]) VALUES (2, N'درصدی')
ƒ
SET IDENTITY_INSERT [dbo].[tblWageCalculationType] OFF
ƒ
SET IDENTITY_INSERT [dbo].[tblWageType] ON 

ƒ
INSERT [dbo].[tblWageType] ([Id], [WageType]) VALUES (1, N'بدون کارمزد')
ƒ
INSERT [dbo].[tblWageType] ([Id], [WageType]) VALUES (2, N'کسر از مبلغ وام')
ƒ
INSERT [dbo].[tblWageType] ([Id], [WageType]) VALUES (3, N'در اولین قسط')
ƒ
INSERT [dbo].[tblWageType] ([Id], [WageType]) VALUES (4, N'طی تمام اقساط')
ƒ
INSERT [dbo].[tblWageType] ([Id], [WageType]) VALUES (5, N'در آخرین قسط')
ƒ
SET IDENTITY_INSERT [dbo].[tblWageType] OFF
ƒ
/****** Object:  StoredProcedure [dbo].[spAllData]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE PROCEDURE[dbo].[spAllData]
AS
BEGIN
DECLARE @LoanFundIniBal BIGINT/*موجودی اولیه صندوق*/
DECLARE @BankAccIniBal BIGINT/*جمع همه موجودی های اولیه حساب های بانکی صندوق*/
DECLARE @Pay BIGINT/*جمع همه پرداخت ها*/
DECLARE @AllChMoPay BIGINT/*جمع همه شارژ ماهانه های پرداخت شده*/
DECLARE @Receivee BIGINT/*جمع همه برداشت ها*/
DECLARE @IncomeAmount BIGINT/*جمع همه درآمد های صندوق*/
DECLARE @FeeAmount BIGINT/*جمع همه هزینه های صندوق*/
DECLARE @InsTotalAmount BIGINT/*جمع همه اقساط پرداخت شده*/
DECLARE @InsTotalDueAmount BIGINT/* جمع ههمه وام ها با احتساب کارمزدها*/
DECLARE @InsRemaining BIGINT/*جمع اقساط پرداخت نشده*/
DECLARE @AllLoanPay BIGINT/*جمع همه وام ها بدون کارمزد*/
DECLARE @LoanPayType1 BIGINT/*جمع همه وام های اقساطی*/
DECLARE @LoanPayType2 BIGINT/*جمع همه وام های مضاربه ای*/
DECLARE @LoanPayType3 BIGINT/*جمع همه وام های ضروری*/
DECLARE @AllWage BIGINT/*جمع همه کارمزد ها*/
DECLARE @AllMembershipFee BIGINT/*جمع همه حق عضویت ها*/
DECLARE @PerMemNum INT /*تعداد اعضا*/
DECLARE @PerNotMemNum INT /*تعداد غیر اعضا*/
DECLARE @AllLoanNum INT/*تعداد کل وام های پرداخت شده*/
DECLARE @LoanNumType1 INT/*تعداد وام اقساطی*/
DECLARE @LoanNumType2 INT/*تعداد وام مضاربه ای*/
DECLARE @LoanNumType3 INT/*تعداد وام ضروری*/
DECLARE @LoanIsPayOff INT/*وام های تسویه شده*/
DECLARE @LoanNotPayOff INT/*وام های تسویه نشده*/
DECLARE @tblAllData table
(TotalFund BIGINT, LoanFundIniBal BIGINT, BankAccIniBal BIGINT, Pay BIGINT, AllMembershipFee BIGINT, AllChMoPay BIGINT, Receivee BIGINT, IncomeAmount BIGINT, FeeAmount BIGINT, InsTotalAmount BIGINT,
 InsTotalDueAmount BIGINT, InsRemaining BIGINT, AllLoanPay BIGINT, LoanPayType1 BIGINT, LoanPayType2 BIGINT, LoanPayType3 BIGINT, AllWage BIGINT,
 PerMemNum INT, PerNotMemNum INT, AllLoanNum INT, LoanNumType1 INT, LoanNumType2 INT, LoanNumType3 INT, LoanIsPayOff INT, LoanNotPayOff INT)

SELECT @PerMemNum = COUNT(*)

FROM dbo.tblPersonnel

WHERE dbo.tblPersonnel.PersonnelMembership = '1'


SELECT @PerNotMemNum = COUNT(*)

FROM dbo.tblPersonnel
WHERE dbo.tblPersonnel.PersonnelMembership = '2'


SELECT @AllLoanNum = COUNT(*)

FROM dbo.tblLoan

SELECT @LoanNumType1 = COUNT(*)

FROM dbo.tblLoan
WHERE dbo.tblLoan.Loan_LoanType_Id = 1


SELECT @LoanNumType2 = COUNT(*)

FROM dbo.tblLoan
WHERE dbo.tblLoan.Loan_LoanType_Id = 2


SELECT @LoanNumType3 = COUNT(*)

FROM dbo.tblLoan
WHERE dbo.tblLoan.Loan_LoanType_Id = 3


SELECT @LoanIsPayOff = COUNT(*)

FROM dbo.tblLoan
WHERE dbo.tblLoan.LoanPayOff = 1


SELECT @LoanNotPayOff = COUNT(*)

FROM dbo.tblLoan
WHERE dbo.tblLoan.LoanPayOff = 0


SELECT @LoanFundIniBal = ISNULL(LoanFundInitialBalance, 0)

FROM dbo.tblLoanFund

SELECT @BankAccIniBal = ISNULL(SUM(CAST(BankAccountInitialBalance AS BIGINT)), 0)

FROM dbo.tblBankAccount

SELECT @Pay = ISNULL(SUM(CAST(AccountAmount AS BIGINT)), 0)

FROM dbo.tblAccount
WHERE Account_TransactionType_Id IN (1,2,3,4,9,10)


SELECT @AllMembershipFee = ISNULL(SUM(CAST(AccountAmount AS BIGINT)), 0)

FROM dbo.tblAccount

WHERE Account_TransactionType_Id IN (11)


SELECT @AllChMoPay = ISNULL(SUM(CAST(AccountAmount AS BIGINT)), 0)

FROM dbo.tblAccount

WHERE Account_TransactionType_Id IN (3)


SELECT @Receivee = ISNULL(SUM(CAST(AccountAmount AS BIGINT)), 0)

FROM dbo.tblAccount

WHERE Account_TransactionType_Id IN (6)


SELECT @IncomeAmount = ISNULL(SUM(CAST(FeeIncomeAmount AS BIGINT)), 0)

FROM dbo.tblFeeIncome

WHERE FeeIncome_FeeIncomeType_Id IN (1)


SELECT @FeeAmount = ISNULL(SUM(CAST(FeeIncomeAmount AS BIGINT)), 0)

FROM dbo.tblFeeIncome

WHERE FeeIncome_FeeIncomeType_Id IN (2)


SELECT @InsTotalAmount = ISNULL(SUM(CAST(InstallmentAmount AS BIGINT)), 0)

FROM dbo.tblInstallment

WHERE InstallmentAmount IS NOT NULL

SELECT @InsTotalDueAmount = ISNULL(SUM(CAST(InstallmentDueAmount AS BIGINT)), 0)

FROM dbo.tblInstallment

SELECT @InsRemaining = ISNULL(SUM(CAST(InstallmentDueAmount AS BIGINT)), 0)

FROM dbo.tblInstallment
WHERE InstallmentAmount IS NULL

SELECT @AllLoanPay = ISNULL(SUM(CAST(LoanAmount AS BIGINT)), 0)

FROM dbo.tblLoan

SELECT @LoanPayType1 = ISNULL(SUM(CAST(LoanAmount AS BIGINT)), 0)

FROM dbo.tblLoan
WHERE Loan_LoanType_Id = 1


SELECT @LoanPayType2 = ISNULL(SUM(CAST(LoanAmount AS BIGINT)), 0)

FROM dbo.tblLoan
WHERE Loan_LoanType_Id = 2


SELECT @LoanPayType3 = ISNULL(SUM(CAST(LoanAmount AS BIGINT)), 0)

FROM dbo.tblLoan
WHERE Loan_LoanType_Id = 3


SELECT @AllWage = ISNULL(SUM(CAST(WageAmount AS BIGINT)), 0)

FROM dbo.tblWage
WHERE WageAmount IS NOT NULL


insert into @tblAllData
     ( TotalFund ,
          LoanFundIniBal ,
          BankAccIniBal ,
          Pay ,
          AllMembershipFee ,
          AllChMoPay ,
          Receivee ,
          IncomeAmount ,
          FeeAmount ,
          InsTotalAmount ,
          InsTotalDueAmount ,
          InsRemaining ,
          AllLoanPay ,
          LoanPayType1 ,
          LoanPayType2 ,
          LoanPayType3 ,
          AllWage ,
          PerMemNum ,
          PerNotMemNum ,
          AllLoanNum ,
          LoanNumType1 ,
          LoanNumType2 ,
          LoanNumType3 ,
          LoanIsPayOff ,
          LoanNotPayOff

        )

VALUES(@LoanFundIniBal + @BankAccIniBal + @Pay + @AllMembershipFee + @IncomeAmount + @InsTotalAmount - @Receivee - @FeeAmount ,
          @LoanFundIniBal ,
          @BankAccIniBal ,
          @Pay ,
          @AllMembershipFee ,
          @AllChMoPay ,
          @Receivee ,
          @IncomeAmount ,
          @FeeAmount ,
          @InsTotalAmount ,
          @InsTotalDueAmount ,
          @InsRemaining ,
          @AllLoanPay ,
          @LoanPayType1 ,
          @LoanPayType2 ,
          @LoanPayType3 ,
          @AllWage ,
          @PerMemNum ,
          @PerNotMemNum ,
          @AllLoanNum ,
          @LoanNumType1 ,
          @LoanNumType2 ,
          @LoanNumType3 ,
          @LoanIsPayOff ,
          @LoanNotPayOff

        )


SELECT* FROM @tblAllData
END
ƒ
/****** Object:  StoredProcedure [dbo].[spAutoAccountId]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE PROCEDURE [dbo].[spAutoAccountId] AS BEGIN DECLARE @a INT = 0; DECLARE @b INT; SET @b = IDENT_CURRENT ( 'tblPerAccType' ) + 1 WHILE   @a = 0 BEGIN IF EXISTS(SELECT TOP 1 Id FROM dbo.tblPerAccType WHERE dbo.tblPerAccType.PerAccTypeAccountNumber = CONVERT(NVARCHAR(10),@b)) SET @b = @b+1 ELSE SET @a = 1 END SELECT @b END  
ƒ
/****** Object:  StoredProcedure [dbo].[spAutoPersonnelId]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE PROCEDURE [dbo].[spAutoPersonnelId] AS BEGIN DECLARE @a INT = 0; DECLARE @b INT; SET @b = IDENT_CURRENT ( 'tblPersonnel' ) + 1 WHILE   @a = 0 BEGIN IF EXISTS(SELECT TOP 1 Id  FROM dbo.tblPersonnel WHERE dbo.tblPersonnel.PersonnelId = CONVERT(NVARCHAR(10),@b)) SET @b = @b+1 ELSE SET @a = 1 END SELECT @b END  
ƒ
/****** Object:  StoredProcedure [dbo].[spPerCanGetMoney]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE PROCEDURE [dbo].[spPerCanGetMoney]  	@PersonnelId INT, 	@InfoId INT  AS BEGIN 	DECLARE @Pay BIGINT 	DECLARE @Receive BIGINT 	DECLARE @CurrentBalance BIGINT 	DECLARE @GuaBlockAmount BIGINT 	DECLARE @InsTotalAmount BIGINT 	DECLARE @InsTotalDueAmount BIGINT 	DECLARE @InsRemaining BIGINT 	DECLARE @CanReceive BIGINT  	SELECT @Pay = ISNULL(sum(cast(AccountAmount as BIGINT)), 0) 	FROM dbo.tblAccount  	WHERE Account_TransactionType_Id IN (1,2,3,4,5,9,10) AND  	Account_Personnel_Id = @PersonnelId 	 	SELECT @Receive = ISNULL(SUM(CAST(AccountAmount AS BIGINT)), 0)  	FROM dbo.tblAccount  	WHERE Account_TransactionType_Id IN (6) AND  	Account_Personnel_Id = @PersonnelId  	SELECT @GuaBlockAmount = ISNULL(SUM(CAST(GuarantorBlockAmount AS BIGINT)), 0) 	FROM dbo.tblGuarantor  	WHERE Guarantor_Info_Id = @InfoId AND  	GuarantorBlockAmount IS NOT NULL AND  	GuarantorBlock = 1   	SELECT @InsTotalAmount = ISNULL(SUM(CAST(InstallmentAmount AS BIGINT)), 0) 	FROM dbo.viewInsLoan  	WHERE Guarantor_Info_Id = @InfoId AND  	InstallmentAmount IS NOT NULL AND 	LoanPayOff = 0 AND 	Guarantor_BlockType_Id = 2  	SELECT @InsTotalDueAmount = ISNULL(SUM(CAST(InstallmentDueAmount AS BIGINT)), 0) 	FROM dbo.viewInsLoan  	WHERE Guarantor_Info_Id = @InfoId AND  	InstallmentDueAmount IS NOT NULL AND 	LoanPayOff = 0 AND 	Guarantor_BlockType_Id = 2  	SET @InsRemaining = @InsTotalDueAmount - @InsTotalAmount  	SET @CurrentBalance = @Pay - @Receive  	SET @CanReceive = @CurrentBalance - @InsRemaining - @GuaBlockAmount  	SELECT @CanReceive END  
ƒ
/****** Object:  StoredProcedure [dbo].[spSelectAccountInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE PROCEDURE [dbo].[spSelectAccountInfo]  	@PersonnelId INT  AS BEGIN  	SELECT * FROM dbo.viewAccountInfo 	WHERE Account_Personnel_Id = @PersonnelId ORDER BY AccountPaymentDate,Id  END  
ƒ
/****** Object:  StoredProcedure [dbo].[spSelectChMoPayInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE PROCEDURE [dbo].[spSelectChMoPayInfo]  	@PersonnelId INT  AS BEGIN  	SELECT * FROM dbo.viewChMoPayInfo 	WHERE Account_Personnel_Id = @PersonnelId  END  
ƒ
/****** Object:  StoredProcedure [dbo].[spSelectFeeIncomeInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE PROCEDURE [dbo].[spSelectFeeIncomeInfo]  AS BEGIN  	SELECT * FROM dbo.viewFeeIncomeInfo  END  
ƒ
/****** Object:  StoredProcedure [dbo].[spSelectLoanInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE PROCEDURE [dbo].[spSelectLoanInfo]  	@PersonnelId INT  AS BEGIN  	SELECT * FROM dbo.viewLoanInfo 	WHERE Loan_Personnel_Id = @PersonnelId  END  
ƒ
/****** Object:  StoredProcedure [dbo].[spSelectPersonnelInfo]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE PROCEDURE [dbo].[spSelectPersonnelInfo]  AS BEGIN  	SELECT * FROM dbo.viewPersonnelInfo  END  
ƒ
/****** Object:  StoredProcedure [dbo].[spSortAccount]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
CREATE PROCEDURE [dbo].[spSortAccount]

	@Account_Personnel_Id INT,
	@AccountPaymentDate NVARCHAR(10)

AS
BEGIN

	DECLARE @PerAccLastTra NVARCHAR(10);
	DECLARE @Count INT = (SELECT COUNT(*) FROM dbo.tblAccount WHERE Account_Personnel_Id = @Account_Personnel_Id);
	SET @PerAccLastTra = (SELECT AccountPaymentDate
	FROM (SELECT TOP 2147483647 AccountPaymentDate, ROW_NUMBER()
	OVER (ORDER BY AccountPaymentDate,Id) AS Rownumber
	FROM dbo.tblAccount WHERE Account_Personnel_Id = @Account_Personnel_Id ORDER BY AccountPaymentDate,Id) AS foo
	WHERE foo.Rownumber = @Count)

	IF (@PerAccLastTra > @AccountPaymentDate)
		EXECUTE spUpdateAccountCurrentBalance @PersonnelId = @Account_Personnel_Id;

END
ƒ
/****** Object:  StoredProcedure [dbo].[spUpdateAccountCurrentBalance]    Script Date: 11/1/2017 11:54:12 AM ******/
SET ANSI_NULLS ON
ƒ
SET QUOTED_IDENTIFIER ON
ƒ
 CREATE PROCEDURE [dbo].[spUpdateAccountCurrentBalance] 	@PersonnelId INT AS BEGIN 	DECLARE @PerAcc TABLE (Id INT,Amount nvarchar(13),CurrentBalance nvarchar(16),PaymentDate NVARCHAR(10), PersonnelId INT, TransactionTypeId INT); 	DECLARE @Total BIGINT = 0; 	DECLARE @Count INT = (SELECT COUNT(*) FROM dbo.tblAccount WHERE Account_Personnel_Id = @PersonnelId); 	DECLARE @TCount INT = 1; 	DECLARE @TTCount INT  	WHILE @count > 0 	BEGIN 		INSERT INTO @PerAcc SELECT Id, AccountAmount, AccountCurrentBalance, AccountPaymentDate, Account_Personnel_Id, Account_TransactionType_Id 		FROM (SELECT TOP 2147483647 Id, AccountAmount, AccountCurrentBalance, AccountPaymentDate, Account_Personnel_Id, Account_TransactionType_Id, ROW_NUMBER() 		OVER (ORDER BY AccountPaymentDate,Id) AS Rownumber 		FROM dbo.tblAccount WHERE Account_Personnel_Id = @PersonnelId ORDER BY AccountPaymentDate,Id) AS foo 		WHERE foo.Rownumber = @TCount; 		IF (SELECT TOP 1 TransactionTypeId FROM @PerAcc) = 6 			SET @Total = @Total - (SELECT SUM(CAST(Amount AS BIGINT)) FROM @PerAcc); 		ELSE 			SET @Total = @Total + (SELECT SUM(CAST(Amount AS BIGINT)) FROM @PerAcc); 		SET @TCount = @TCount + 1; 		SET @TTCount = (SELECT TOP 1 Id FROM @PerAcc)  		UPDATE dbo.tblAccount SET AccountCurrentBalance = @Total 		WHERE Id = @TTCount 		 		SET @Count = @Count - 1 		DELETE FROM @PerAcc 	END END  
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00] Begin DesignProperties =     Begin PaneConfigurations =        Begin PaneConfiguration = 0          NumPanes = 4          Configuration = "(H (1[40] 4[20] 2[20] 3) )"       End       Begin PaneConfiguration = 1          NumPanes = 3          Configuration = "(H (1 [50] 4 [25] 3))"       End       Begin PaneConfiguration = 2          NumPanes = 3          Configuration = "(H (1 [50] 2 [25] 3))"       End       Begin PaneConfiguration = 3          NumPanes = 3          Configuration = "(H (4 [30] 2 [40] 3))"       End       Begin PaneConfiguration = 4          NumPanes = 2          Configuration = "(H (1 [56] 3))"       End       Begin PaneConfiguration = 5          NumPanes = 2          Configuration = "(H (2 [66] 3))"       End       Begin PaneConfiguration = 6          NumPanes = 2          Configuration = "(H (4 [50] 3))"       End       Begin PaneConfiguration = 7          NumPanes = 1          Configuration = "(V (3))"       End       Begin PaneConfiguration = 8          NumPanes = 3          Configuration = "(H (1[56] 4[18] 2) )"       End       Begin PaneConfiguration = 9          NumPanes = 2          Configuration = "(H (1 [75] 4))"       End       Begin PaneConfiguration = 10          NumPanes = 2          Configuration = "(H (1[66] 2) )"       End       Begin PaneConfiguration = 11          NumPanes = 2          Configuration = "(H (4 [60] 2))"       End       Begin PaneConfiguration = 12          NumPanes = 1          Configuration = "(H (1) )"       End       Begin PaneConfiguration = 13          NumPanes = 1          Configuration = "(V (4))"       End       Begin PaneConfiguration = 14          NumPanes = 1          Configuration = "(V (2))"       End       ActivePaneConfig = 0    End    Begin DiagramPane =        Begin Origin =           Top = 0          Left = 0       End       Begin Tables =           Begin Table = "tblAccount"             Begin Extent =                 Top = 39                Left = 40                Bottom = 169                Right = 282             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblTransactionType"             Begin Extent =                 Top = 90                Left = 455                Bottom = 186                Right = 632             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblPaymentType"             Begin Extent =                 Top = 0                Left = 313                Bottom = 96                Right = 483             End             DisplayFlags = 280             TopColumn = 0          End       End    End    Begin SQLPane =     End    Begin DataPane =        Begin ParameterDefaults = ""       End       Begin ColumnWidths = 12          Width = 284          Width = 1500          Width = 2460          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500       End    End    Begin CriteriaPane =        Begin ColumnWidths = 11          Column = 1440          Alias = 900          Table = 1170          Output = 720          Append = 1400          NewValue = 1170          SortType = 1350          SortOrder = 1410          GroupBy = 1350          Filter = 1350          Or = 1350          Or = 1350          Or = 1350       End    End End ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewAccountInfo'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewAccountInfo'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00] Begin DesignProperties =     Begin PaneConfigurations =        Begin PaneConfiguration = 0          NumPanes = 4          Configuration = "(H (1[47] 4[18] 2[22] 3) )"       End       Begin PaneConfiguration = 1          NumPanes = 3          Configuration = "(H (1 [50] 4 [25] 3))"       End       Begin PaneConfiguration = 2          NumPanes = 3          Configuration = "(H (1 [50] 2 [25] 3))"       End       Begin PaneConfiguration = 3          NumPanes = 3          Configuration = "(H (4 [30] 2 [40] 3))"       End       Begin PaneConfiguration = 4          NumPanes = 2          Configuration = "(H (1 [56] 3))"       End       Begin PaneConfiguration = 5          NumPanes = 2          Configuration = "(H (2 [66] 3))"       End       Begin PaneConfiguration = 6          NumPanes = 2          Configuration = "(H (4 [50] 3))"       End       Begin PaneConfiguration = 7          NumPanes = 1          Configuration = "(V (3))"       End       Begin PaneConfiguration = 8          NumPanes = 3          Configuration = "(H (1[56] 4[18] 2) )"       End       Begin PaneConfiguration = 9          NumPanes = 2          Configuration = "(H (1 [75] 4))"       End       Begin PaneConfiguration = 10          NumPanes = 2          Configuration = "(H (1[66] 2) )"       End       Begin PaneConfiguration = 11          NumPanes = 2          Configuration = "(H (4 [60] 2))"       End       Begin PaneConfiguration = 12          NumPanes = 1          Configuration = "(H (1) )"       End       Begin PaneConfiguration = 13          NumPanes = 1          Configuration = "(V (4))"       End       Begin PaneConfiguration = 14          NumPanes = 1          Configuration = "(V (2))"       End       ActivePaneConfig = 0    End    Begin DiagramPane =        Begin Origin =           Top = 0          Left = 0       End       Begin Tables =           Begin Table = "tblAccount"             Begin Extent =                 Top = 6                Left = 38                Bottom = 136                Right = 280             End             DisplayFlags = 280             TopColumn = 5          End          Begin Table = "tblTransactionType"             Begin Extent =                 Top = 140                Left = 310                Bottom = 236                Right = 487             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblPaymentType"             Begin Extent =                 Top = 110                Left = 712                Bottom = 206                Right = 882             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblChMoPay"             Begin Extent =                 Top = 0                Left = 444                Bottom = 130                Right = 688             End             DisplayFlags = 280             TopColumn = 2          End       End    End    Begin SQLPane =     End    Begin DataPane =        Begin ParameterDefaults = ""       End    End    Begin CriteriaPane =        Begin ColumnWidths = 11          Column = 1440          Alias = 570          Table = 1110          Output = 765          Append = 1400          NewValue = 1170          SortType = 1350          SortOrder = 1410          GroupBy = 1350          Filter = 1350          Or = 1350          Or = 1350          Or = 1350       End    End End ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewChMoPayInfo'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewChMoPayInfo'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00] Begin DesignProperties =     Begin PaneConfigurations =        Begin PaneConfiguration = 0          NumPanes = 4          Configuration = "(H (1[40] 4[20] 2[20] 3) )"       End       Begin PaneConfiguration = 1          NumPanes = 3          Configuration = "(H (1 [50] 4 [25] 3))"       End       Begin PaneConfiguration = 2          NumPanes = 3          Configuration = "(H (1 [50] 2 [25] 3))"       End       Begin PaneConfiguration = 3          NumPanes = 3          Configuration = "(H (4 [30] 2 [40] 3))"       End       Begin PaneConfiguration = 4          NumPanes = 2          Configuration = "(H (1 [56] 3))"       End       Begin PaneConfiguration = 5          NumPanes = 2          Configuration = "(H (2 [66] 3))"       End       Begin PaneConfiguration = 6          NumPanes = 2          Configuration = "(H (4 [50] 3))"       End       Begin PaneConfiguration = 7          NumPanes = 1          Configuration = "(V (3))"       End       Begin PaneConfiguration = 8          NumPanes = 3          Configuration = "(H (1[56] 4[18] 2) )"       End       Begin PaneConfiguration = 9          NumPanes = 2          Configuration = "(H (1 [75] 4))"       End       Begin PaneConfiguration = 10          NumPanes = 2          Configuration = "(H (1[66] 2) )"       End       Begin PaneConfiguration = 11          NumPanes = 2          Configuration = "(H (4 [60] 2))"       End       Begin PaneConfiguration = 12          NumPanes = 1          Configuration = "(H (1) )"       End       Begin PaneConfiguration = 13          NumPanes = 1          Configuration = "(V (4))"       End       Begin PaneConfiguration = 14          NumPanes = 1          Configuration = "(V (2))"       End       ActivePaneConfig = 0    End    Begin DiagramPane =        Begin Origin =           Top = 0          Left = 0       End       Begin Tables =           Begin Table = "tblFeeIncome"             Begin Extent =                 Top = 6                Left = 38                Bottom = 136                Right = 289             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblFeeIncomeType"             Begin Extent =                 Top = 6                Left = 327                Bottom = 102                Right = 500             End             DisplayFlags = 280             TopColumn = 0          End       End    End    Begin SQLPane =     End    Begin DataPane =        Begin ParameterDefaults = ""       End    End    Begin CriteriaPane =        Begin ColumnWidths = 11          Column = 1440          Alias = 900          Table = 1170          Output = 720          Append = 1400          NewValue = 1170          SortType = 1350          SortOrder = 1410          GroupBy = 1350          Filter = 1350          Or = 1350          Or = 1350          Or = 1350       End    End End ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewFeeIncomeInfo'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewFeeIncomeInfo'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00] Begin DesignProperties =     Begin PaneConfigurations =        Begin PaneConfiguration = 0          NumPanes = 4          Configuration = "(H (1[41] 4[10] 2[23] 3) )"       End       Begin PaneConfiguration = 1          NumPanes = 3          Configuration = "(H (1 [50] 4 [25] 3))"       End       Begin PaneConfiguration = 2          NumPanes = 3          Configuration = "(H (1 [50] 2 [25] 3))"       End       Begin PaneConfiguration = 3          NumPanes = 3          Configuration = "(H (4 [30] 2 [40] 3))"       End       Begin PaneConfiguration = 4          NumPanes = 2          Configuration = "(H (1 [56] 3))"       End       Begin PaneConfiguration = 5          NumPanes = 2          Configuration = "(H (2 [66] 3))"       End       Begin PaneConfiguration = 6          NumPanes = 2          Configuration = "(H (4 [50] 3))"       End       Begin PaneConfiguration = 7          NumPanes = 1          Configuration = "(V (3))"       End       Begin PaneConfiguration = 8          NumPanes = 3          Configuration = "(H (1[56] 4[18] 2) )"       End       Begin PaneConfiguration = 9          NumPanes = 2          Configuration = "(H (1 [75] 4))"       End       Begin PaneConfiguration = 10          NumPanes = 2          Configuration = "(H (1[66] 2) )"       End       Begin PaneConfiguration = 11          NumPanes = 2          Configuration = "(H (4 [60] 2))"       End       Begin PaneConfiguration = 12          NumPanes = 1          Configuration = "(H (1) )"       End       Begin PaneConfiguration = 13          NumPanes = 1          Configuration = "(V (4))"       End       Begin PaneConfiguration = 14          NumPanes = 1          Configuration = "(V (2))"       End       ActivePaneConfig = 0    End    Begin DiagramPane =        Begin Origin =           Top = -96          Left = 0       End       Begin Tables =           Begin Table = "tblLoan"             Begin Extent =                 Top = 6                Left = 38                Bottom = 136                Right = 277             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblInstallment"             Begin Extent =                 Top = 6                Left = 315                Bottom = 136                Right = 556             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblGuarantor"             Begin Extent =                 Top = 138                Left = 38                Bottom = 268                Right = 280             End             DisplayFlags = 280             TopColumn = 0          End       End    End    Begin SQLPane =     End    Begin DataPane =        Begin ParameterDefaults = ""       End       Begin ColumnWidths = 9          Width = 284          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500       End    End    Begin CriteriaPane =        Begin ColumnWidths = 11          Column = 1440          Alias = 900          Table = 1170          Output = 720          Append = 1400          NewValue = 1170          SortType = 1350          SortOrder = 1410          GroupBy = 1350          Filter = 1350          Or = 1350          Or = 1350          Or = 1350       End    End End ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewInsLoan'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewInsLoan'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00] Begin DesignProperties =     Begin PaneConfigurations =        Begin PaneConfiguration = 0          NumPanes = 4          Configuration = "(H (1[40] 4[20] 2[20] 3) )"       End       Begin PaneConfiguration = 1          NumPanes = 3          Configuration = "(H (1 [50] 4 [25] 3))"       End       Begin PaneConfiguration = 2          NumPanes = 3          Configuration = "(H (1 [50] 2 [25] 3))"       End       Begin PaneConfiguration = 3          NumPanes = 3          Configuration = "(H (4 [30] 2 [40] 3))"       End       Begin PaneConfiguration = 4          NumPanes = 2          Configuration = "(H (1 [56] 3))"       End       Begin PaneConfiguration = 5          NumPanes = 2          Configuration = "(H (2 [66] 3))"       End       Begin PaneConfiguration = 6          NumPanes = 2          Configuration = "(H (4 [50] 3))"       End       Begin PaneConfiguration = 7          NumPanes = 1          Configuration = "(V (3))"       End       Begin PaneConfiguration = 8          NumPanes = 3          Configuration = "(H (1[56] 4[18] 2) )"       End       Begin PaneConfiguration = 9          NumPanes = 2          Configuration = "(H (1 [75] 4))"       End       Begin PaneConfiguration = 10          NumPanes = 2          Configuration = "(H (1[66] 2) )"       End       Begin PaneConfiguration = 11          NumPanes = 2          Configuration = "(H (4 [60] 2))"       End       Begin PaneConfiguration = 12          NumPanes = 1          Configuration = "(H (1) )"       End       Begin PaneConfiguration = 13          NumPanes = 1          Configuration = "(V (4))"       End       Begin PaneConfiguration = 14          NumPanes = 1          Configuration = "(V (2))"       End       ActivePaneConfig = 0    End    Begin DiagramPane =        Begin Origin =           Top = 0          Left = 0       End       Begin Tables =           Begin Table = "tblLoan"             Begin Extent =                 Top = 74                Left = 41                Bottom = 204                Right = 280             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblLoanType"             Begin Extent =                 Top = 109                Left = 574                Bottom = 205                Right = 744             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblWage"             Begin Extent =                 Top = 0                Left = 309                Bottom = 130                Right = 564             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblWageType"             Begin Extent =                 Top = 34                Left = 764                Bottom = 130                Right = 934             End             DisplayFlags = 280             TopColumn = 0          End       End    End    Begin SQLPane =     End    Begin DataPane =        Begin ParameterDefaults = ""       End    End    Begin CriteriaPane =        Begin ColumnWidths = 11          Column = 1440          Alias = 900          Table = 1170          Output = 720          Append = 1400          NewValue = 1170          SortType = 1350          SortOrder = 1410          GroupBy = 1350          Filter = 1350          Or = 1350          Or = 1350          Or = 1350       End    End End ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewLoanInfo'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewLoanInfo'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00] Begin DesignProperties =     Begin PaneConfigurations =        Begin PaneConfiguration = 0          NumPanes = 4          Configuration = "(H (1[40] 4[20] 2[20] 3) )"       End       Begin PaneConfiguration = 1          NumPanes = 3          Configuration = "(H (1 [50] 4 [25] 3))"       End       Begin PaneConfiguration = 2          NumPanes = 3          Configuration = "(H (1 [50] 2 [25] 3))"       End       Begin PaneConfiguration = 3          NumPanes = 3          Configuration = "(H (4 [30] 2 [40] 3))"       End       Begin PaneConfiguration = 4          NumPanes = 2          Configuration = "(H (1 [56] 3))"       End       Begin PaneConfiguration = 5          NumPanes = 2          Configuration = "(H (2 [66] 3))"       End       Begin PaneConfiguration = 6          NumPanes = 2          Configuration = "(H (4 [50] 3))"       End       Begin PaneConfiguration = 7          NumPanes = 1          Configuration = "(V (3))"       End       Begin PaneConfiguration = 8          NumPanes = 3          Configuration = "(H (1[56] 4[18] 2) )"       End       Begin PaneConfiguration = 9          NumPanes = 2          Configuration = "(H (1 [75] 4))"       End       Begin PaneConfiguration = 10          NumPanes = 2          Configuration = "(H (1[66] 2) )"       End       Begin PaneConfiguration = 11          NumPanes = 2          Configuration = "(H (4 [60] 2))"       End       Begin PaneConfiguration = 12          NumPanes = 1          Configuration = "(H (1) )"       End       Begin PaneConfiguration = 13          NumPanes = 1          Configuration = "(V (4))"       End       Begin PaneConfiguration = 14          NumPanes = 1          Configuration = "(V (2))"       End       ActivePaneConfig = 0    End    Begin DiagramPane =        Begin Origin =           Top = 0          Left = 0       End       Begin Tables =           Begin Table = "tblPersonnel"             Begin Extent =                 Top = 6                Left = 38                Bottom = 136                Right = 270             End             DisplayFlags = 280             TopColumn = 0          End          Begin Table = "tblInfo"             Begin Extent =                 Top = 7                Left = 441                Bottom = 137                Right = 629             End             DisplayFlags = 280             TopColumn = 0          End       End    End    Begin SQLPane =     End    Begin DataPane =        Begin ParameterDefaults = ""       End       Begin ColumnWidths = 30          Width = 284          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500          Width = 1500       End    End    Begin CriteriaPane =        Begin ColumnWidths = 11          Column = 1440          Alias = 900          Table = 1170          Output = 720          Append = 1400          NewValue = 1170          SortType = 1350          SortOrder = 1410          GroupBy = 1350          Filter = 1350          Or = 1350          Or = 1350          Or = 1350       End    End End ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewPersonnelInfo'
ƒ
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewPersonnelInfo'
ƒ
USE [master]
ƒ
ALTER DATABASE [dbLoan] SET  READ_WRITE 
ƒ
