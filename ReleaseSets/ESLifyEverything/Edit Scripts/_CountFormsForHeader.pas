unit _CountFormsForHeader;

interface
  implementation
  uses xEditAPI, Classes, SysUtils, StrUtils, Windows;

var
  NewRecords, TotalCount: Integer;
  previousStr: string;

function Initialize: integer;
begin
  previousStr := 'Initialized';
  NewRecords := 0;
  TotalCount := 0;
end;

function Process(e: IInterface): integer;
begin
	if IsMaster(e) then begin
		Inc(TotalCount);
		if BaseName(GetFile(e)) <> previousStr then begin
			AddMessage(previousStr + ': ' + IntToStr(NewRecords));
			previousStr := BaseName(GetFile(e));
			NewRecords := 0;
		end;
		Inc(NewRecords);
		
	end;
end;

function Finalize: integer;
begin
  AddMessage(previousStr + ': ' + IntToStr(NewRecords));
  AddMessage('Total Records Counted: ' + IntToStr(TotalCount));
end;

end.
