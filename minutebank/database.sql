USE [minutebankDev]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 10/01/2024 11:50:53 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account_number] [nvarchar](24) NOT NULL,
	[balance] [bigint] NOT NULL,
	[type] [nvarchar](50) NOT NULL,
	[status] [bit] NOT NULL,
	[user_id] [int] NOT NULL,
 CONSTRAINT [PK_Account_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Card]    Script Date: 10/01/2024 11:50:53 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Card](
	[card_number] [char](16) NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	[expiry] [date] NOT NULL,
	[cvc] [char](3) NOT NULL,
	[account_id] [int] NOT NULL,
 CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Credit]    Script Date: 10/01/2024 11:50:53 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Credit](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NOT NULL,
	[amount] [bigint] NOT NULL,
	[from_account_number] [nvarchar](24) NULL,
	[payment_request_id] [int] NULL,
	[account_id] [int] NOT NULL,
 CONSTRAINT [PK_Credit] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Debit]    Script Date: 10/01/2024 11:50:53 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Debit](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NOT NULL,
	[amount] [bigint] NOT NULL,
	[recipient_id] [int] NULL,
	[account_id] [int] NOT NULL,
	[to_account_number] [nvarchar](24) NULL,
 CONSTRAINT [PK_Debit] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentRequest]    Script Date: 10/01/2024 11:50:53 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentRequest](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sent_to] [nvarchar](255) NOT NULL,
	[amount] [bigint] NOT NULL,
	[due_by] [datetime] NOT NULL,
	[status] [bit] NOT NULL,
	[account_number] [nvarchar](24) NOT NULL,
	[user_id] [int] NULL,
 CONSTRAINT [PK_PaymentRequest] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipient]    Script Date: 10/01/2024 11:50:53 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipient](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[bank] [nvarchar](255) NOT NULL,
	[swift_code] [smallint] NOT NULL,
	[account_number] [nvarchar](24) NOT NULL,
	[user_id] [int] NULL,
 CONSTRAINT [PK_Recipient] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/01/2024 11:50:53 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_balance]  DEFAULT ((0)) FOR [balance]
GO
ALTER TABLE [dbo].[Card] ADD  CONSTRAINT [DF_Card_card_number]  DEFAULT ((0)) FOR [card_number]
GO
ALTER TABLE [dbo].[Credit] ADD  CONSTRAINT [DF_Credit_amount]  DEFAULT ((0)) FOR [amount]
GO
ALTER TABLE [dbo].[PaymentRequest] ADD  CONSTRAINT [DF_PaymentRequest_status]  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[PaymentRequest] ADD  CONSTRAINT [DF_PaymentRequest_account_number]  DEFAULT ((0)) FOR [account_number]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_User]
GO
ALTER TABLE [dbo].[Card]  WITH CHECK ADD  CONSTRAINT [FK_Card_Account] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[Card] CHECK CONSTRAINT [FK_Card_Account]
GO
ALTER TABLE [dbo].[Credit]  WITH CHECK ADD  CONSTRAINT [FK_Credit_Account] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[Credit] CHECK CONSTRAINT [FK_Credit_Account]
GO
ALTER TABLE [dbo].[Credit]  WITH CHECK ADD  CONSTRAINT [FK_Credit_PaymentRequest] FOREIGN KEY([payment_request_id])
REFERENCES [dbo].[PaymentRequest] ([id])
GO
ALTER TABLE [dbo].[Credit] CHECK CONSTRAINT [FK_Credit_PaymentRequest]
GO
ALTER TABLE [dbo].[Debit]  WITH CHECK ADD  CONSTRAINT [FK_Debit_Account] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[Debit] CHECK CONSTRAINT [FK_Debit_Account]
GO
ALTER TABLE [dbo].[Debit]  WITH CHECK ADD  CONSTRAINT [FK_Debit_Recipient] FOREIGN KEY([recipient_id])
REFERENCES [dbo].[Recipient] ([id])
GO
ALTER TABLE [dbo].[Debit] CHECK CONSTRAINT [FK_Debit_Recipient]
GO
ALTER TABLE [dbo].[PaymentRequest]  WITH CHECK ADD  CONSTRAINT [FK_PaymentRequest_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[PaymentRequest] CHECK CONSTRAINT [FK_PaymentRequest_User]
GO
ALTER TABLE [dbo].[Recipient]  WITH CHECK ADD  CONSTRAINT [FK_Recipient_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Recipient] CHECK CONSTRAINT [FK_Recipient_User]
GO
/****** Object:  StoredProcedure [dbo].[AccountStats]    Script Date: 10/01/2024 11:50:53 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[AccountStats] (@user_id INT)
AS
SELECT 'debit' as [type],[date], Debit.amount as [amount], [to_account_number] as [account], Recipient.name as [name], Recipient.account_number as [counterparty_account] from Debit 
INNER JOIN [Account] ON Debit.account_id = Account.id 
INNER JOIN [Recipient] ON Debit.recipient_id = Recipient.id
WHERE Account.user_id = @user_id
UNION
SELECT 'credit' as [type],[date], Credit.amount as [amount], Account.account_number as [account], PaymentRequest.sent_to as [name], PaymentRequest.account_number as [counterparty_account] from Credit 
INNER JOIN [Account] ON Credit.account_id = Account.id
INNER JOIN [PaymentRequest] ON Credit.payment_request_id = PaymentRequest.id
WHERE Account.user_id = @user_id
ORDER BY [date];
GO
/****** Object:  StoredProcedure [dbo].[Summary]    Script Date: 10/01/2024 11:50:53 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Summary] (@user_id INT)
AS
BEGIN
SELECT SUM(Credit.amount) as credit, SUM(Debit.amount) as debit, SUM(Account.balance) as balance FROM [User]
INNER JOIN Account ON Account.user_id = [User].id
LEFT JOIN Debit ON Account.id = Debit.account_id
LEFT JOIN Credit ON Account.id =  Credit.account_id
WHERE user_id = @user_id
END

GO
