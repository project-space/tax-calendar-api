if exists (select 1 from Event where Id = @Id)
begin
    update Event
        set FirmId = @FirmId,
            State = @State,
            Start = @Start,
            [End]   = @End,
            EntityId = @EntityId,
            EntityType = @EntityType
end
else begin
    insert into Event (FirmId, State, Start, [End], EntityId, EntityType)
        values (@FirmId, @State, @Start, @End, @EntityId, @EntityType)
end