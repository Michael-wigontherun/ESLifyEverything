unit _ESPFEEverythingFaceGenFix;

interface
  implementation
  uses xEditAPI, mteFunctions, _wotrFunctions, Classes, SysUtils, StrUtils, Windows;



function Initialize: integer;
var
  Lines, LineList: TStringList;
  intI : Integer;
begin
  Lines := TStringList.Create;
  Lines.LoadFromFile('.\FaceGenEslIfyFix.txt');
  LineList := TStringList.Create;
  LineList.Delimiter := ';';
  LineList.StrictDelimiter := True;
  for intI := 0 to Lines.Count -1 do
  begin
    LineList.DelimitedText := Lines[intI];
    CreateFaceMesh(LineList[0],LineList[0], LineList[1], LineList[2]);
  end;
  Lines.Clear;
  Lines.free;
  LineList.Clear;
  LineList.free;
  //CreateFaceMesh('.\meshes\00000800.nif','.\meshes\00000800.nif', '000022F1', '00000800');
end;

procedure CreateFaceMesh(MeshOldPath, MeshNewPath, OldFormID, NewFormID : string);
var
    Nif              : TwbNifFile;
    Block            : TwbNifBlock;
    el               : TdfElement;
    Elements         : TList;
    i, j, k          : Integer;
    s, s2            : WideString;
    bChanged         : Boolean;
begin
    Nif := TwbNifFile.Create;
    Nif.LoadFromFile(MeshOldPath);
    
    Elements := TList.Create;
    
    // Iterate over all blocks in a nif file and locate elements holding textures.
    for i := 0 to Pred(Nif.BlocksCount) do begin
        Block := Nif.Blocks[i];
        
        if Block.BlockType = 'BSShaderTextureSet' then begin
            el := Block.Elements['Textures'];
            for j := 0 to Pred(el.Count) do
                Elements.Add(el[j]);
        end; 
    end;
    
    AddMessage(Format('Found %d elements.', [Elements.Count]));

    // Skip to the next file if nothing was found.
    if Elements.Count = 0 then Exit;
    
    // Do text replacement in collected elements.
    for k := 0 to Pred(Elements.Count) do begin
        if not Assigned(Elements[k]) then Continue
        el := TdfElement(Elements[k]);
        
        // Getting file name stored in element.
        s := el.EditValue;
        // Skip to the next element if empty.
        if s = '' then Continue;
        
        // Perform replacements, trim whitespaces just in case.
        s2 := Trim(s);
        s2 := StringReplace(s2, OldFormID, NewFormID, [rfIgnoreCase, rfReplaceAll]);
        
        // If element's value has changed.
        if s <> s2 then begin
            // Store it.
            el.EditValue := s2;
            
            // Report.
            //if Verbose then AddMessage(#13#10 + MeshOldPath);
            //if Verbose then AddMessage(#9 + el.Path + #13#10#9#9'"' + s + '"'#13#10#9#9'"' + el.EditValue + '"');
        end;
        

		// Create the same folders structure as the source file in the destination folder.
		s := ExtractFilePath(MeshNewPath);
		if not DirectoryExists(s) then
			if not ForceDirectories(s) then
				raise Exception.Create('Can not create destination directory ' + s);
	
		// Get the root of the last processed element (the file element itself) and save.
		el.Root.SaveToFile(MeshNewPath);
    end;
    
    // Clear mark and elements list.
    bChanged := False;
    Elements.Clear;    
    Elements.Free;
    Nif.Free;    
end;

end.