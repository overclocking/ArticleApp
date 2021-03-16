--게시판 테이블
CREATE TABLE [dbo].[Articles]
(
	[Id]	INT NOT NULL PRIMARY KEY Identity(1,1),	--일련번호
	[Title] NVarChar(255) NOT NULL,					--제목
	
	-- TODO: Columns Add Region
	[Content] NVarChar(Max) NOT NULL,					--내용


	-- AuditableBase.cs 참조
	[CreatedBy]  Nvarchar(255) null,				--생성자(Creator)
	[Created]	 DateTime Default(GetDate()),		--생성일
	[ModifiedBy] NVarChar(255) null,				--수정자(LastModifiedBy)
	[Modified]	 DateTime null,						--수정일(LastModified)
)
Go