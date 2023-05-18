unit _ESLifyModSplit_Step3;

interface
  implementation
  uses xEditAPI, Classes, SysUtils, StrUtils, Windows;

function Process(e: IInterface): integer;
var 
  edid: string;
  removePos: integer;
begin
  if IsMaster(e) = false then Exit;
  edid := GetElementEditValues(e, 'EDID');
  removePos := pos('_ESLifySplit', edid) - 1;
  edid := Copy(edid, 0, removePos);

  if(removePos > 0) then SetEditValue(ElementByPath(e, 'EDID'), edid);
end;

end.