#/usr/bin/bash

HangulCharLookup="가간감갖개게경계공과금기끔나너녕노높뉴느는니님다더도동됩뒤드든디딩때래랭레려력로록료를리릭림마막만많머멀메며면명모문뮤받방버벨보북브쁘사새서설세셔셨소속수순쉽스습시십아악안않야어에예오와완요용운원위으은을음이익인임입있자작잘잠재저전점접정제좋주죽중즘지직차체총취커켬코큐크클키킹타터텐통트튼티포표프플하할합해했향화환회효희히"

echo "<Font>" > font.xml

echo "$HangulCharLookup" | while IFS= read -r -n1 ch; do
    echo "  <Char Character=\"$ch\" Width=\"9\" />" >> font.xml
done

echo "</Font>" >> font.xml
