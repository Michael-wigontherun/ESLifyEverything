unit _ESLifyModSplit_Step2;

interface
  implementation
  uses xEditAPI, Classes, SysUtils, StrUtils, Windows;

var
  SplitList: TStringList;

function Initialize: integer;
begin
  SplitList := TStringList.Create;
end;

//pluginName;FormID;EDID
function Process(e: IInterface): integer;
begin
  if IsMaster(e) = false then Exit;
  SplitList.Add(Format('%s;:;%s;:;%s', [
    BaseName(GetFile(e)),
    IntToHex(FixedFormID(e), 8),
    GetElementEditValues(e, 'EDID')
  ]));
end;

procedure createOutputFolder(folderDirectory: TStringList);
var 
  i : integer;
begin
  for i := 0 to folderDirectory.Count -1 do
  begin 
    if not DirectoryExists(folderDirectory[i]) then 
    begin 
      if not CreateDir(folderDirectory[i]) then AddMessage('Could not create "' + folderDirectory[i] + '" Directory.')
      else AddMessage('Created "' + folderDirectory[i] + '" Directory.');
    end;
  end;
end;

function Finalize: integer;
var folderDirectory : TStringList;
begin
  folderDirectory := TStringList.Create;
  folderDirectory.add('ESLifyEverything');
  folderDirectory.add('ESLifyEverything\xEditOutput');
  createOutputFolder(folderDirectory);
  if not Assigned(SplitList) then
    Exit;
  if (SplitList.Count > 0) then begin
      SplitList.SaveToFile(ProgramPath+'ESLifyEverything\xEditOutput\SplitData.csv');
  end;

  SplitList.Free;
  
 end;

end.