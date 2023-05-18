unit _ESLifyModSplit_Step1;

interface
  implementation
  uses xEditAPI, Classes, SysUtils, StrUtils, Windows;

var
  count : integer;
  OrgList: TStringList;

function Initialize: integer;
begin
  count := 0;
  OrgList := TStringList.Create;
end;

//pluginName;FormID;EDID
function Process(e: IInterface): integer;
var edid: string;
begin
  if IsMaster(e) = false then Exit;
  
  edid := GetElementEditValues(e, 'EDID') + '_ESLifySplit' + IntToStr(count);
  SetElementEditValues(e, 'EDID', edid);
  
  OrgList.Add(Format('%s;:;%s;:;%s', [
      BaseName(GetFile(e)),
      IntToHex(FixedFormID(e), 8),
      edid
    ]));
  Inc(count);
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
  if not Assigned(OrgList) then
    Exit;
  if (OrgList.Count > 0) then begin
      OrgList.SaveToFile(ProgramPath+'ESLifyEverything\xEditOutput\OriginalData.csv');
  end;

  OrgList.Free;
  
 end;

end.