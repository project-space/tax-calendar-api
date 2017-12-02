if exists (select 1 from Setting where FirmId = @FirmId)
    begin
        update Setting set
            [Values] = @Values
    end
    else begin
        insert into Setting
            values (@FirmId, @Values)
    end