/*AppVersion 1.1.0*/ƒ

IF NOT EXISTS ( SELECT 1 FROM dbo.tblTransactionType WHERE Id = 11 )
BEGIN
	DBCC CHECKIDENT ('[tblTransactionType]', RESEED, 10);
    INSERT INTO dbo.tblTransactionType
            ( TransactionType )
    VALUES  ( N'حق عضویت')
END
ELSE
BEGIN
	UPDATE dbo.tblTransactionType SET TransactionType = N'حق عضویت' WHERE Id = 11
END

ƒ

ALTER TABLE dbo.tblLoan ADD Loan_Account_Id INT

ƒ

ALTER VIEW [dbo].[viewLoanInfo]
AS
SELECT        dbo.tblLoan.Id, dbo.tblLoan.Loan_Personnel_Id, dbo.tblLoan.Loan_Account_Id,dbo.tblLoan.Loan_LoanType_Id, dbo.tblLoan.LoanAmount, dbo.tblLoan.LoanDate, dbo.tblLoan.LoanInstallmentNum, dbo.tblLoan.LoanInstallmentFirstPayDate, 
                         dbo.tblLoan.LoanInstallmentInterspace, dbo.tblLoan.LoanPayOff, dbo.tblLoan.LoanDescription, dbo.tblLoanType.LoanType, dbo.tblWage.Wage_WageType_Id, dbo.tblWage.Wage_WageCalculationType_Id, 
                         dbo.tblWage.WagePercent, dbo.tblWage.WageAmount, dbo.tblWage.WageDescription, dbo.tblWageType.WageType
FROM            dbo.tblLoan INNER JOIN
                         dbo.tblLoanType ON dbo.tblLoanType.Id = dbo.tblLoan.Loan_LoanType_Id INNER JOIN
                         dbo.tblWage ON dbo.tblWage.Wage_Loan_Id = dbo.tblLoan.Id INNER JOIN
                         dbo.tblWageType ON dbo.tblWageType.Id = dbo.tblWage.Wage_WageType_Id

ƒ

CREATE PROCEDURE[dbo].[spPeriodAllData]

@StartDate NVARCHAR(10),
@EndDate NVARCHAR(10)

AS
BEGIN
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
DECLARE @AllMembershipFee BIGINT/*جمع همه حق عضویت ها*/
DECLARE @PerMemNum INT /*تعداد اعضا*/
DECLARE @PerNotMemNum INT /*تعداد غیر اعضا*/
DECLARE @AllLoanNum INT/*تعداد کل وام های پرداخت شده*/
DECLARE @LoanNumType1 INT/*تعداد وام اقساطی*/
DECLARE @LoanNumType2 INT/*تعداد وام مضاربه ای*/
DECLARE @LoanNumType3 INT/*تعداد وام ضروری*/
DECLARE @LoanIsPayOff INT/*وام های تسویه شده*/
DECLARE @LoanNotPayOff INT/*وام های تسویه نشده*/
DECLARE @tblPeriodAllData table
(TotalFund BIGINT, Pay BIGINT, AllMembershipFee BIGINT, AllChMoPay BIGINT, Receivee BIGINT, IncomeAmount BIGINT, FeeAmount BIGINT, InsTotalAmount BIGINT,
 InsTotalDueAmount BIGINT, InsRemaining BIGINT, AllLoanPay BIGINT, LoanPayType1 BIGINT, LoanPayType2 BIGINT, LoanPayType3 BIGINT,
 PerMemNum INT, PerNotMemNum INT, AllLoanNum INT, LoanNumType1 INT, LoanNumType2 INT, LoanNumType3 INT, LoanIsPayOff INT, LoanNotPayOff INT)

SELECT @PerMemNum = COUNT(*)

FROM dbo.tblPersonnel

WHERE dbo.tblPersonnel.PersonnelMembership = '1' AND PersonnelMembershipDate >= @StartDate AND PersonnelMembershipDate <= @EndDate


SELECT @PerNotMemNum = COUNT(*)

FROM dbo.tblPersonnel
WHERE dbo.tblPersonnel.PersonnelMembership = '2' AND PersonnelMembershipDate >= @StartDate AND PersonnelMembershipDate <= @EndDate


SELECT @AllLoanNum = COUNT(*)

FROM dbo.tblLoan WHERE LoanDate >= @StartDate AND LoanDate <= @EndDate

SELECT @LoanNumType1 = COUNT(*) 

FROM dbo.tblLoan
WHERE dbo.tblLoan.Loan_LoanType_Id = 1 AND LoanDate >= @StartDate AND LoanDate <= @EndDate


SELECT @LoanNumType2 = COUNT(*)

FROM dbo.tblLoan
WHERE dbo.tblLoan.Loan_LoanType_Id = 2 AND LoanDate >= @StartDate AND LoanDate <= @EndDate


SELECT @LoanNumType3 = COUNT(*)

FROM dbo.tblLoan
WHERE dbo.tblLoan.Loan_LoanType_Id = 3 AND LoanDate >= @StartDate AND LoanDate <= @EndDate


SELECT @LoanIsPayOff = COUNT(*) 

FROM dbo.tblLoan
WHERE dbo.tblLoan.LoanPayOff = 1 AND LoanDate >= @StartDate AND LoanDate <= @EndDate


SELECT @LoanNotPayOff = COUNT(*)

FROM dbo.tblLoan
WHERE dbo.tblLoan.LoanPayOff = 0 AND LoanDate >= @StartDate AND LoanDate <= @EndDate


SELECT @Pay = ISNULL(SUM(CAST(AccountAmount AS BIGINT)), 0)

FROM dbo.tblAccount
WHERE Account_TransactionType_Id IN (1,2,3,4,9,10) AND AccountPaymentDate >= @StartDate AND AccountPaymentDate <= @EndDate


SELECT @AllMembershipFee = ISNULL(SUM(CAST(AccountAmount AS BIGINT)), 0)

FROM dbo.tblAccount

WHERE Account_TransactionType_Id IN (11) AND AccountPaymentDate >= @StartDate AND AccountPaymentDate <= @EndDate


SELECT @AllChMoPay = ISNULL(SUM(CAST(AccountAmount AS BIGINT)), 0)

FROM dbo.tblAccount

WHERE Account_TransactionType_Id IN (3) AND AccountPaymentDate >= @StartDate AND AccountPaymentDate <= @EndDate


SELECT @Receivee = ISNULL(SUM(CAST(AccountAmount AS BIGINT)), 0)

FROM dbo.tblAccount

WHERE Account_TransactionType_Id IN (6) AND AccountPaymentDate >= @StartDate AND AccountPaymentDate <= @EndDate


SELECT @IncomeAmount = ISNULL(SUM(CAST(FeeIncomeAmount AS BIGINT)), 0)

FROM dbo.tblFeeIncome

WHERE FeeIncome_FeeIncomeType_Id IN (1) AND FeeIncomeDate >= @StartDate AND FeeIncomeDate <= @EndDate


SELECT @FeeAmount = ISNULL(SUM(CAST(FeeIncomeAmount AS BIGINT)), 0)

FROM dbo.tblFeeIncome

WHERE FeeIncome_FeeIncomeType_Id IN (2) AND FeeIncomeDate >= @StartDate AND FeeIncomeDate <= @EndDate


SELECT @InsTotalAmount = ISNULL(SUM(CAST(InstallmentAmount AS BIGINT)), 0)

FROM dbo.tblInstallment

WHERE InstallmentAmount IS NOT NULL AND InstallmentPaymentDate >= @StartDate AND InstallmentPaymentDate <= @EndDate


SELECT @InsTotalDueAmount = ISNULL(SUM(CAST(InstallmentDueAmount AS BIGINT)), 0)

FROM dbo.tblInstallment 
WHERE InstallmentPaymentDate >= @StartDate AND InstallmentPaymentDate <= @EndDate


SELECT @InsRemaining = ISNULL(SUM(CAST(InstallmentDueAmount AS BIGINT)), 0)

FROM dbo.tblInstallment
WHERE InstallmentAmount IS NULL AND InstallmentPaymentDate >= @StartDate AND InstallmentPaymentDate <= @EndDate

SELECT @AllLoanPay = ISNULL(SUM(CAST(LoanAmount AS BIGINT)), 0)

FROM dbo.tblLoan WHERE LoanDate >= @StartDate AND LoanDate <= @EndDate

SELECT @LoanPayType1 = ISNULL(SUM(CAST(LoanAmount AS BIGINT)), 0)

FROM dbo.tblLoan
WHERE Loan_LoanType_Id = 1 AND LoanDate >= @StartDate AND LoanDate <= @EndDate


SELECT @LoanPayType2 = ISNULL(SUM(CAST(LoanAmount AS BIGINT)), 0)

FROM dbo.tblLoan
WHERE Loan_LoanType_Id = 2 AND LoanDate >= @StartDate AND LoanDate <= @EndDate


SELECT @LoanPayType3 = ISNULL(SUM(CAST(LoanAmount AS BIGINT)), 0)

FROM dbo.tblLoan
WHERE Loan_LoanType_Id = 3 AND LoanDate >= @StartDate AND LoanDate <= @EndDate


insert into @tblPeriodAllData
     ( TotalFund ,
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
          PerMemNum ,
          PerNotMemNum ,
          AllLoanNum ,
          LoanNumType1 ,
          LoanNumType2 ,
          LoanNumType3 ,
          LoanIsPayOff ,
          LoanNotPayOff

        )

VALUES(@Pay + @AllMembershipFee + @IncomeAmount + @InsTotalAmount - @Receivee - @FeeAmount ,
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
          @PerMemNum ,
          @PerNotMemNum ,
          @AllLoanNum ,
          @LoanNumType1 ,
          @LoanNumType2 ,
          @LoanNumType3 ,
          @LoanIsPayOff ,
          @LoanNotPayOff

        )


SELECT* FROM @tblPeriodAllData
END

ƒ

ALTER PROCEDURE [dbo].[spSortAccount]

	@Account_Personnel_Id INT

AS
BEGIN

	DECLARE @PerAccLastTra NVARCHAR(10);
	DECLARE @Count INT = (SELECT COUNT(*) FROM dbo.tblAccount WHERE Account_Personnel_Id = @Account_Personnel_Id);
	SET @PerAccLastTra = (SELECT AccountPaymentDate
	FROM (SELECT TOP 2147483647 AccountPaymentDate, ROW_NUMBER()
	OVER (ORDER BY AccountPaymentDate,Id) AS Rownumber
	FROM dbo.tblAccount WHERE Account_Personnel_Id = @Account_Personnel_Id ORDER BY AccountPaymentDate,Id) AS foo
	WHERE foo.Rownumber = @Count)

	EXECUTE spUpdateAccountCurrentBalance @PersonnelId = @Account_Personnel_Id;

END
