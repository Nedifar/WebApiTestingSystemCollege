import './App.css';

import { createReactEditorJS } from 'react-editor-js';
import edjsHTML from 'editorjs-html';

import { useRef, useCallback } from 'react';
import { EDITOR_JS_TOOLS } from './constants';

const ReactEditorJS = createReactEditorJS({
    theme: 'dark'
});
const edjsParser = edjsHTML();

function App() {
    const defaultValue = {
        "time": 1636709084205,
        "blocks": [
        ],
        "version": "2.22.2"
    }
    const editorJS = useRef(null);
    const handleInitialize = useCallback((instance) => {
        editorJS.current = instance;
    }, []);

    const handleSave = useCallback(async () => {
        const savedData = await editorJS.current.save();
        const html = edjsParser.parse(savedData);
        let formatHtml = '';
        html.forEach(element => {
            formatHtml += element + '\n';
        });
        console.log(html);
        console.log(formatHtml);
        console.log(savedData);

    }, []);

    const handleClear = useCallback(async () => {
        await editorJS.current.clear();
    }, []);
    return (
        <div>
            <h3>
                Задание
            </h3>
            
            <h3>
                Номер тестового набора
            </h3>
            <input id='idTestPack'/>
            <h3>
                Ответ
            </h3>
            <input id='answear'/>
            <h3>
                Баллы
            </h3>
            <input id='score' type='number'/>
            <h3>
                Решение
            </h3>
            //???
            <h3>
                Штраф за неправильный ответ
            </h3>
            <input id='fine' type='number'/>
            <h3>
                Тема
            </h3>
            <input id='theme'/>
            <h3>
                Повышенная сложность
            </h3>
            <input id='isComplexity' type='checkbox'/>
            <h3>
                Порядок важен
            </h3>
            <input id='isOrderImportant' type='checkbox'/>
            <h3>
                Тип ответа
            </h3>
            <input id='ansType' name='ansType' type='radio' />
            <input id='ansType' name='ansType' type='radio'/>
            <input id='ansType' name='ansType' type='radio'/>
            <h3>
                Режим вставки
            </h3>
            <select id='insertMode'>
                <option value='start'>В начало</option>
                <option value='end'>В конец</option>
                <option value='special'>По номеру</option>
            </select>
            
            <ReactEditorJS
                tools={EDITOR_JS_TOOLS}
                onInitialize={handleInitialize}
                defaultValue={defaultValue}

            // readOnly={true}
            />
            <button onClick={handleSave}>Save</button>
            <button onClick={handleClear}>Clear</button>
        </div>
    );
}

export default App;
