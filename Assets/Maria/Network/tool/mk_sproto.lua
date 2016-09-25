local filename = "./../../../service/cat/proto.lua"
local fd = io.open(filename, "r")
local buffer = fd:read("a")
fd:close()
local c = {}
local x = 1

local b = string.find(buffer, "%[%[")
local e = string.find(buffer, "%]%]")
local c2s = string.sub(buffer, b+3, e-2)
fd = io.open("c2s.sproto", "w")
fd:write(c2s)
fd:close()

b = string.find(buffer, "%[%[", e)
e = string.find(buffer, "%]%]", b)

local s2c = string.sub(buffer, b+3, e-2)
fd = io.open("s2c.sproto", "w")
fd:write(s2c)
fd:close()
