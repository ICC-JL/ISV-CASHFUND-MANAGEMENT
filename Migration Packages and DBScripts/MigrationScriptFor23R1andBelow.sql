-- Fill out NoteID fields for ATPTEFMFundEstablishment
UPDATE dbo.ATPTEFMFundEstablishment
SET NoteID = NEWID()
WHERE NoteID IS NULL;

-- Rename the old index if it exists
IF EXISTS (
    SELECT 1
    FROM sys.indexes i
    INNER JOIN sys.tables t ON i.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = N'ATPTEFMFundEstablishment'
      AND s.name = N'dbo'
      AND i.name = N'ATPTEFMFundsApprovalSetup_NoteID'
)
Begin
	EXEC sp_rename N'dbo.ATPTEFMFundEstablishment.ATPTEFMFundsApprovalSetup_NoteID', N'ATPTEFMFundEstablishment_NoteID', 'INDEX'
End

-- Fill out NoteID fields for ATPTEFMFundTransactionSetupApproval
 UPDATE dbo.ATPTEFMFundTransactionSetupApproval
SET NoteID = NEWID()
WHERE NoteID IS NULL;

