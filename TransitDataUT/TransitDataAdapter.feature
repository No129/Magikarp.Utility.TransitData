Feature: TransitDataAdapter
	為提供資料傳遞過程採用中介資料傳遞，
	操作及使用時採用資料物件，
	提供資料物件轉換為中介資料及從中介資料載入為資料物件的轉換器功能。

Scenario: 轉換 Employee 資料物件為字串中介資料
	Given 僱員資料
	| Name          | Level | Title |
	| Tomozou Huang | 1     | Staff |
	When 要求轉換為字串中介資料
	Then 得到字串中介資料
	"""
	<Root>
		<Name>Tomozou Huang</Name>
		<Level>1</Level>
		<Title>Staff</Title>
	</Root>
	"""

Scenario: 轉字串中介資料為 Employee 資料物件
	Given 字串中介資料
		"""
		<Root>
			<Name>Tomozou Huang</Name>
			<Level>1</Level>
			<Title>Staff</Title>
		</Root>
		"""
	When 要求轉換為 Employee 資料物件
	Then 得到 Employee 資料物件
		| Name          | Level | Title |
		| Tomozou Huang | 1     | Staff |

