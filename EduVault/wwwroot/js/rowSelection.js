let selectedRow = null;
let selectedId = null;

function selectRow(row, id) {
    // Снимаем выделение со всех строк
    document.querySelectorAll('.clickable-row').forEach(r => {
        r.classList.remove('table-active');
    });

    // Выделяем текущую строку
    row.classList.add('table-active');
    selectedRow = row;
    selectedId = id;

    // Активируем кнопки
    document.getElementById('editBtn').disabled = false;
    document.getElementById('deleteBtn').disabled = false;

    // Обновляем ID для форм
    document.getElementById('deleteId').value = id;

    // Обновляем action формы редактирования (удаляем старый ID если был)
    const editForm = document.getElementById('editForm');
    editForm.action = editForm.action.split('?')[0] + '?id=' + id;
}

// Очистка выделения при клике вне таблицы (опционально)
document.addEventListener('click', function (e) {
    if (!e.target.closest('.clickable-row') && !e.target.closest('.btn')) {
        clearSelection();
    }
});

function clearSelection() {
    if (selectedRow) {
        selectedRow.classList.remove('table-active');
        selectedRow = null;
        selectedId = null;

        // Деактивируем кнопки
        document.getElementById('editBtn').disabled = true;
        document.getElementById('deleteBtn').disabled = true;
    }
}
