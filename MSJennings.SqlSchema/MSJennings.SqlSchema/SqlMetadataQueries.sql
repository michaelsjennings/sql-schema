-- 0: tables

select
	schema_name(tbl.schema_id) as table_schema_name,
	tbl.name as table_name,
	col.name as column_name,
	typ.name as type_name,
	col.max_length,
	col.precision,
	col.scale,
	col.is_nullable,
	col.is_identity,
	col.is_computed
	
from
	sys.tables tbl
	
	join sys.columns col
		on col.object_id = tbl.object_id
		
	join sys.types typ
		on typ.user_type_id = col.system_type_id

where
	tbl.type_desc = 'USER_TABLE'
	and
	tbl.name not in ('sysdiagrams', 'RefactorLog')

order by
	table_schema_name,
	table_name,
	col.column_id

-- 1: indexes

select
	schema_name(ix_tbl.schema_id) as index_schema_name,
	ix_tbl.name as index_table_name,
	ix.name as index_name,
	ix.is_primary_key,
	ix.is_unique,
	cast(case when ix.index_id = 1 then 1 else 0 end as bit) as is_clustered,
	ix_col.name as index_column_name,
	ixc.is_included_column,
	ix.is_disabled

from
	sys.indexes ix

	join sys.index_columns ixc
		on ixc.object_id = ix.object_id
		and ixc.index_id = ix.index_id
		
	join sys.tables ix_tbl
		on ix_tbl.object_id = ix.object_id
		and ix_tbl.type_desc = 'USER_TABLE'
		and ix_tbl.name not in ('sysdiagrams', 'RefactorLog')

	join sys.columns ix_col
		on ix_col.object_id = ixc.object_id
		and ix_col.column_id = ixc.column_id

order by
	index_schema_name,
	index_table_name,
	index_name,
	case when ixc.key_ordinal > 0 then ixc.key_ordinal else 99999 end,
	case when ixc.key_ordinal < 1 then ix_col.name else 'aaaaa' end

-- 2: foreign keys

select
	schema_name(fk_tbl.schema_id) as foreign_key_schema_name,
	fk_tbl.name as foreign_key_table_name,
	fk.name as foreign_key_name,
	fk_col.name as foreign_key_column_name,
	schema_name(pk_tbl.schema_id) as primary_key_schema_name,
	pk_tbl.name as primary_key_table_name,
	pk_col.name as primary_key_column_name,
	fk.is_disabled
	
from
	sys.foreign_keys fk

	join sys.foreign_key_columns fkc
		on fkc.parent_object_id = fk.parent_object_id
		and fkc.constraint_object_id = fk.object_id
		
	join sys.tables fk_tbl
		on fk_tbl.object_id = fk.parent_object_id
		and fk_tbl.type_desc = 'USER_TABLE'
		and fk_tbl.name not in ('sysdiagrams', '_RefactorLog')

	join sys.columns fk_col
		on fk_col.object_id = fkc.parent_object_id
		and fk_col.column_id = fkc.parent_column_id

	join sys.tables pk_tbl
		on pk_tbl.object_id = fk.referenced_object_id
		
	join sys.columns pk_col
		on pk_col.object_id = fkc.referenced_object_id
		and pk_col.column_id = fkc.referenced_column_id

order by
	foreign_key_schema_name,
	foreign_key_table_name,
	foreign_key_name,
	fkc.constraint_column_id

-- 3: check constraints

select
	schema_name(tbl.schema_id) as check_constraint_schema_name,
	tbl.name as check_constraint_table_name,
	col.name as check_constraint_column_name,
	chk.name as check_constraint_name,
	replace(
	replace(
		object_definition(chk.object_id),
		'[', ''),
		']', '')
		as check_constraint_definition,
	chk.is_disabled

from
	sys.check_constraints chk

	join sys.tables tbl
		on tbl.object_id = chk.parent_object_id
		and tbl.type_desc = 'USER_TABLE'
		and tbl.name not in ('sysdiagrams', 'RefactorLog')

	join sys.columns col
		on col.object_id = chk.parent_object_id
		and col.column_id = chk.parent_column_id

	order by
		check_constraint_schema_name,
		check_constraint_table_name,
		check_constraint_column_name

-- 4: default constraints

select
	schema_name(tbl.schema_id) as default_constraint_schema_name,
	tbl.name as default_constraint_table_name,
	col.name as default_constraint_column_name,
	def.name as default_constraint_name,
	replace(
	replace(
		object_definition(def.object_id),
		'[', ''),
		']', '')
		as default_constraint_definition
	
from
	sys.default_constraints def

	join sys.tables tbl
		on tbl.object_id = def.parent_object_id
		and tbl.type_desc = 'USER_TABLE'
		and tbl.name not in ('sysdiagrams', 'RefactorLog')
		
	join sys.columns col
		on col.object_id = def.parent_object_id
		and col.column_id = def.parent_column_id

order by
	default_constraint_schema_name,
	default_constraint_table_name,
	default_constraint_column_name

-- 5: triggers

select
	schema_name(tbl.schema_id) as trigger_schema_name,
	tbl.name as trigger_table_name,
	trg.name as trigger_name,
	object_definition (trg.object_id) as trigger_definition,
	trg.is_disabled

from
	sys.triggers trg

	join sys.tables tbl
		on tbl.object_id = trg.parent_id
		and tbl.type_desc = 'USER_TABLE'
		and tbl.name not in ('sysdiagrams', 'RefactorLog')

order by
	trigger_schema_name,
	trigger_table_name,
	trigger_name

-- 6: views

select
	schema_name(vw.schema_id) as view_schema_name,
	vw.name as view_name,
	col.name as column_name,
	typ.name as type_name,
	col.max_length,
	col.precision,
	col.scale,
	col.is_nullable,
	col.is_identity
	
from
	sys.views vw

	join sys.columns col
		on col.object_id = vw.object_id

	join sys.types typ
		on typ.user_type_id = col.system_type_id

order by
	view_schema_name,
	view_name,
	col.column_id

-- 7: stored procedures

select
	schema_name(sp.schema_id) as stored_procedure_schema_name,
	sp.name as stored_procedure_name,
	object_definition (sp.object_id) as stored_procedure_definition
	
from
	sys.procedures sp

where
	sp.name not like 'sp[_]%'

order by
	stored_procedure_schema_name,
	stored_procedure_name

-- 8: stored procedure parameters

select
	schema_name(sp.schema_id) as stored_procedure_schema_name,
	sp.name as stored_procedure_name,
	param.name as parameter_name,
	typ.name as type_name,
	param.max_length,
	param.precision,
	param.scale,
	param.is_output,
	param. default_value
	
from
	sys.procedures sp
	
	join sys.parameters param
		on param.object_id = sp.object_id

	join sys.types typ
		on typ.user_type_id = param.system_type_id
		
where
	sp.name not like 'sp[_]%'
	
order by
	stored_procedure_schema_name,
	stored_procedure_name,
	param.parameter_id

-- 9: functions

select
	schema_name(fn.schema_id) as function_schema_name,
	fn.name as function_name,
	fn.type_desc,
	object_definition(fn.object_id) as function_definition
	
from
	sys.objects fn
	
where
	fn.type in ('FN', 'IF', 'TF')
	and
	fn.name not like 'fn[_]diag%'
	
order by
	function_schema_name,
	function_name

-- 10: function parameters

select
	schema_name(fn.schema_id) as function_schema_name,
	fn.name as function_name,
	param.name as parameter_name,
	typ.name as type_name,
	param.max_length,
	param.precision,
	param.scale,
	param.is_output,
	param.default_value
	
from
	sys.objects fn
	
	join sys.parameters param
		on param.object_id = fn.object_id

	join sys.types typ
		on typ.user_type_id = param.system_type_id

where
	fn.type in ('FN', 'IF', 'TF')
	and
	fn.name not like 'fn[_]diag%'
	
order by
	function_schema_name,
	function_name,
	param.parameter_id