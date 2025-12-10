USE [armaradio]
GO

/****** Object:  StoredProcedure [dbo].[ArmaUsers_GetEmailConfirmationTokenByEmail]    Script Date: 12/10/2025 3:31:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[ArmaUsers_GetEmailConfirmationTokenByEmail]
(
	@email varchar(450)
)
AS
BEGIN
	set nocount on;
	declare
		@token varchar(150);

	select
		@token = ec.[ConfirmationToken]
	from AspNetUsers us
	join ArmaUsers_EmailConfirmation ec
		on ec.[UserIdentity] = us.[Id]
	where
		us.[Email] = @email;

	select
		[Token] = @token;

	/*
		exec ArmaUsers_GetEmailConfirmationTokenByEmail
			'luis.e.valle@gmail.com'
	*/
END
GO

