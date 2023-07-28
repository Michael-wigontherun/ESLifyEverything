unit _CountFormsForHeader;

interface
  implementation
  uses xEditAPI, Classes, SysUtils, StrUtils, Windows;

var
  NewRecords, TotalCount, Threshhold: Integer;
  previousStr: string;
  ThreshholdList: TStringList;

function Initialize: integer;
begin
  previousStr := 'Initialized';
  NewRecords := 0;
  TotalCount := 0;
  Threshhold := 100;
  ThreshholdList := TStringList.Create;
end;

function Process(e: IInterface): integer;
begin
	if IsMaster(e) then begin
		Inc(TotalCount);
		if BaseName(GetFile(e)) <> previousStr then begin
      if NewRecords > Threshhold then begin
        ThreshholdList.Add(previousStr + ': ' + IntToStr(NewRecords));
      end;
			AddMessage(previousStr + ': ' + IntToStr(NewRecords));
			previousStr := BaseName(GetFile(e));
			NewRecords := 0;
		end;
		Inc(NewRecords);
		
	end;
end;

function Finalize: integer;
var i: integer;
begin
  
  if NewRecords > Threshhold then begin
    ThreshholdList.Add(previousStr + ': ' + IntToStr(NewRecords));
  end;
  AddMessage(previousStr + ': ' + IntToStr(NewRecords));

  AddMessage('Total Records Counted: ' + IntToStr(TotalCount));

  AddMessage('------------------------------------------------------------');
  AddMessage('These broke the threshhold of ' + IntToStr(Threshhold));
  For i := 0 to ThreshholdList.Count - 1 do
  begin
    AddMessage(ThreshholdList[i]);
  end;

end;

end.
