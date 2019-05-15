USE [DBAMP_Dev]
GO

/****** Object:  Table [dbo].[Tokens]    Script Date: 5/22/2017 4:49:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tokens](
	[TokenId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[AuthToken] [nvarchar](250) NOT NULL,
	[IssuedOn] [datetime] NOT NULL,
	[ExpiresOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED 
(
	[TokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Tokens]  WITH CHECK ADD  CONSTRAINT [FK_Tokens_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserID])
GO

ALTER TABLE [dbo].[Tokens] CHECK CONSTRAINT [FK_Tokens_User]
GO


