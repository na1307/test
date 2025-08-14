local dpb, ypb

local colors = {
    day_midnight = '1B1B1B',
    day_sunrise = 'FFCA28',
    day_morning = '6EB3D8',
    day_noon = '00BFFF',
    day_sunset = 'FF8F00',
    day_night = '010445',

    year_winter = 'F9F9FB',
    year_spring = 'F18D9E',
    year_summer = '4BAD3D',
    year_autumn = 'DF7200'
}

function FormatPercentage(value, decimals)
    local multiplier = 10 ^ decimals

    return string.format('%.' .. decimals .. 'f', math.floor(value * multiplier) / multiplier)
end

function Initialize()
    local functions = {
        DayPercentage = function()
            local now = os.date('*t')
            local seconds_today = now.hour * 3600 + now.min * 60 + now.sec + 1

            if dpb then
                local now_hour = math.floor((seconds_today - 1) / 3600)
                local time_map = {
                    -- 한밤
                    [0] = colors.day_midnight, [1] = colors.day_midnight, [2] = colors.day_midnight, [3] = colors.day_midnight, [4] = colors.day_midnight,
                    -- 일출
                    [5] = colors.day_sunrise, [6] = colors.day_sunrise, [7] = colors.day_sunrise,
                    -- 아침
                    [8] = colors.day_morning, [9] = colors.day_morning, [10] = colors.day_morning, [11] = colors.day_morning,
                    -- 한낮
                    [12] = colors.day_noon, [13] = colors.day_noon, [14] = colors.day_noon, [15] = colors.day_noon, [16] = colors.day_noon, [17] = colors.day_noon,
                    -- 일몰
                    [18] = colors.day_sunset, [19] = colors.day_sunset, [20] = colors.day_sunset,
                    -- 밤
                    [21] = colors.day_night, [22] = colors.day_night, [23] = colors.day_night
                }

                SKIN:Bang('!SetOption', 'DayProgressBar', 'BarColor', time_map[now_hour])
            end

            return FormatPercentage(seconds_today / 86400 * 100, 2)
        end,
        YearPercentage = function()
            local days = tonumber(os.date('%j', os.time({year = tonumber(os.date('%Y')), month = 12, day = 31})))
            local today = tonumber(os.date('%j'))

            if ypb then
                local month = tonumber(os.date('%m'))
                local season_map = {
                    -- 겨울
                    [12] = colors.year_winter, [1] = colors.year_winter, [2] = colors.year_winter,
                    -- 봄
                    [3] = colors.year_spring, [4] = colors.year_spring, [5] = colors.year_spring,
                    -- 여름
                    [6] = colors.year_summer, [7] = colors.year_summer, [8] = colors.year_summer,
                    -- 가을
                    [9] = colors.year_autumn, [10] = colors.year_autumn, [11] = colors.year_autumn
                }

                SKIN:Bang('!SetOption', 'YearProgressBar', 'BarColor', season_map[month])
            end

            return FormatPercentage(today / days * 100, 1)
        end
    }

    local option = SELF:GetOption('Function')
    f = functions[option]

    if not f then
        error('The \'' .. option .. '\' function is invalid.')
    end

    dpb = SKIN:GetMeter('DayProgressBar')
    ypb = SKIN:GetMeter('YearProgressBar')
end

function Update()
    if f then
        return f()
    else
        return nil
    end
end
